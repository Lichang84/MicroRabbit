using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Application.Services
{
	public class TransferService : ITransferService
	{
		private readonly ITransferRepository m_oTransferRepository;
		private readonly IEventBus m_bus;

		public TransferService(ITransferRepository transferRepository, IEventBus bus)
		{
			m_oTransferRepository = transferRepository;
			m_bus = bus;
		}

		public IEnumerable<TransferLog> GetTransferLogs()
		{
			return m_oTransferRepository.GetTransferLogs();
		}

	}
}
