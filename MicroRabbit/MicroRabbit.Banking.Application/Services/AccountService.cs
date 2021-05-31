using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Application.Services
{
	public class AccountService : IAccountService
	{
		private readonly IAccountRepository m_oAccountRepository;
		private readonly IEventBus m_bus;

		public AccountService(IAccountRepository accountRepository, IEventBus bus)
		{
			m_oAccountRepository = accountRepository;
			m_bus = bus;
		}

		public IEnumerable<Account> GetAccounts()
		{
			return m_oAccountRepository.GetAccounts();
		}

		public void Transfer(AccountTransfer accountTransfer)
		{
			var createTransferCommand = new CreateTransferCommand(accountTransfer.FromAccount, accountTransfer.ToAccount, accountTransfer.TransferAmount);

			m_bus.SendCommand(createTransferCommand);
		}
	}
}
