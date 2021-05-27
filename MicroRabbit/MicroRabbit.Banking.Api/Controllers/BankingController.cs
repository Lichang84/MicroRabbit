using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.Banking.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BankingController : ControllerBase
	{
		private readonly IAccountService m_accountService;

		public BankingController(IAccountService accountServcie)
		{
			m_accountService = accountServcie;
		}

		// GET: api/banking
		[HttpGet]
		public ActionResult<IEnumerable<Account>> Get()
		{
			return Ok(m_accountService.GetAccounts());
		}

	}
}
