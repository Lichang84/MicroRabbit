using MicroRabbit.MVC.Models.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MicroRabbit.MVC.Services
{
	public class TransferService : ITransferService
	{
		private readonly HttpClient m_apiClient;

		public TransferService(HttpClient apiClient)
		{
			m_apiClient = apiClient;
		}

		public async Task Transfer(TransferDto transferDto)
		{
			var uri = "https://localhost:5001/api/Banking";
			var transferContent = new StringContent(JsonConvert.SerializeObject(transferDto), System.Text.Encoding.UTF8, "application/json");
			var response = await m_apiClient.PostAsync(uri, transferContent);

			response.EnsureSuccessStatusCode();
		}
	}
}
