using MicroRabbit.MVC.Models;
using MicroRabbit.MVC.Models.DTO;
using MicroRabbit.MVC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MicroRabbit.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ITransferService m_transferService;

		public HomeController(ITransferService transferService)
		{
			m_transferService = transferService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Transfer(TransferViewModel model)
		{
			TransferDto transferDto = new TransferDto()
			{
				FromAccount = model.FromAccount,
				ToAccount = model.ToAccount,
				TransferAmount = model.TransferAmount
			};

			await m_transferService.Transfer(transferDto);

			return View("Index");
		}
	}
}
