using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Application.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository m_oAccountRepository;

		public AccountService(IAccountRepository accountRepository)
		{
			m_oAccountRepository = accountRepository;
		}

		public IEnumerable<Account> GetAccounts()
		{
			return m_oAccountRepository.GetAccounts();
		}
	}
}
