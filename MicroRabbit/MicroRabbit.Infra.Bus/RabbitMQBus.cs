using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
	public sealed class RabbitMQBus : IEventBus
	{
		private readonly IMediator m_oMediator;
		private readonly Dictionary<string, List<Type>> m_oHandlers;
		private readonly List<Type> m_oEventTypes;
		private readonly IServiceScopeFactory m_serviceScopeFactory;

		public RabbitMQBus(IMediator oMediator, IServiceScopeFactory sericeScopeFactory)
		{
			m_oMediator = oMediator;
			m_serviceScopeFactory = sericeScopeFactory;
			m_oHandlers = new Dictionary<string, List<Type>>();
			m_oEventTypes = new List<Type>();
		}

		public void Publish<T>(T @event) where T : Event
		{
			var oFactory = new ConnectionFactory() { 
				HostName = "localhost"
			};

			using (var oConnection = oFactory.CreateConnection())
			{
				using (var oChannel = oConnection.CreateModel())
				{
					var sEventName = @event.GetType().Name;

					oChannel.QueueDeclare(sEventName, false, false, false, null);

					var sMessage = JsonConvert.SerializeObject(@event);
					var oBody = Encoding.UTF8.GetBytes(sMessage);

					oChannel.BasicPublish("", sEventName, null, oBody);
				}
			}
		}

		public Task SendCommand<T>(T command) where T : Command
		{
			return m_oMediator.Send(command);
		}

		public void Subscribe<T, TH>()
			where T : Event
			where TH : IEventHandler<T>
		{
			var sEventname = typeof(T).Name;
			var oHandlerType = typeof(TH);

			if (!m_oEventTypes.Contains(typeof(T)))
			{
				m_oEventTypes.Add(typeof(T));
			}

			if (!m_oHandlers.ContainsKey(sEventname))
			{
				m_oHandlers.Add(sEventname, new List<Type>());
			}

			if (m_oHandlers[sEventname].Any(s => s.GetType() == oHandlerType))
			{
				throw new ArgumentException($"Handler type {oHandlerType.Name} already registered for {sEventname}", nameof(oHandlerType));
			}

			m_oHandlers[sEventname].Add(oHandlerType);

			StartBasicConsume<T>();
		}

		private void StartBasicConsume<T>() where T : Event
		{
			var oFactory = new ConnectionFactory()
			{
				HostName = "localhost",
				DispatchConsumersAsync = true
			};

			using (var oConnection = oFactory.CreateConnection())
			{
				using (var oChannel = oConnection.CreateModel())
				{
					var sEventName = typeof(T).Name;

					oChannel.QueueDeclare(sEventName, false, false, false, null);

					var oConsumer = new AsyncEventingBasicConsumer(oChannel);

					oConsumer.Received += Consuer_Received;
					oChannel.BasicConsume(sEventName, true, oConsumer);
				}
			}
		}

		private async Task Consuer_Received(object sender, BasicDeliverEventArgs e)
		{
			var sEventName = e.RoutingKey;
			var sMessage = Encoding.UTF8.GetString(e.Body.ToArray());

			try
			{
				await ProcessEvent(sEventName, sMessage).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				throw;
			}
		}

		private async Task ProcessEvent(string sEventName, string sMessage)
		{
			if (m_oHandlers.ContainsKey(sEventName))
			{
				using (var scope = m_serviceScopeFactory.CreateScope())
				{
					var oSubscriptions = m_oHandlers[sEventName];

					foreach (var subscription in oSubscriptions)
					{
						// var oHandler = Activator.CreateInstance(subscription);
						var oHandler = scope.ServiceProvider.GetService(subscription);

						if (null == oHandler)
						{
							continue;
						}

						var oEventType = m_oEventTypes.SingleOrDefault(t => t.Name == sEventName);
						var @event = JsonConvert.DeserializeObject(sMessage, oEventType);
						var oConcreteType = typeof(IEventHandler<>).MakeGenericType(oEventType);
						await (Task)oConcreteType.GetMethod("Handle").Invoke(oHandler, new object[] { @event });
					}
				}
			}
		}
	}
}
