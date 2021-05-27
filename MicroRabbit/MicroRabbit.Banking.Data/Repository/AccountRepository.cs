using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Data.Repository
{
	public class AccountRepository : IAccountRepository
	{
		private BankingDbContext m_oContext;

		public AccountRepository(BankingDbContext context)
		{
			m_oContext = context;
		}

		public IEnumerable<Account> GetAccounts()
		{
			return m_oContext.Accounts;
		}
	}
}
