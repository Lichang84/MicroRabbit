using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Data.Repository
{
	public class TransferRepository : ITransferRepository
	{
		private TransferDbContext m_oContext;

		public TransferRepository(TransferDbContext context)
		{
			m_oContext = context;
		}

		public void Add(TransferLog transferLog)
		{
			m_oContext.TransferLogs.Add(transferLog);
			m_oContext.SaveChanges();
		}

		public IEnumerable<TransferLog> GetTransferLogs()
		{
			return m_oContext.TransferLogs;
		}
	}
}
