using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.Transfer.Api.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TransferController : Controller
	{
		private readonly ITransferService m_transferService;

		public TransferController (ITransferService transferService)
		{
			m_transferService = transferService;
		}

		// Get api/transfer
		[HttpGet]
		public ActionResult<IEnumerable<TransferLog>> Get()
		{
			return Ok(m_transferService.GetTransferLogs());
		}
	}
}
