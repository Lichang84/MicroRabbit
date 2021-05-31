using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Domain.CommandHandlers
{
	public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
	{
		private readonly IEventBus m_bus;

		public TransferCommandHandler(IEventBus bus)
		{
			m_bus = bus;
		}

		public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
		{
			// Publish event to RabbitMQ
			m_bus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));

			return Task.FromResult(true);
		}
	}
}
