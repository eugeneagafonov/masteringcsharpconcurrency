using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;

namespace QueueServer
{
	internal class Program
	{
		internal const string _baseAddress = "http://localhost:9100/";
		private static void Main(string[] args)
		{

			int n = 2;

			using (WebApp.Start<Startup>(url: baseAddress))
			{
				HttpClient client = new HttpClient();

				var response = client.GetAsync(baseAddress + "api/Home").Result;

				Console.WriteLine(response);
				Console.WriteLine(response.Content.ReadAsStringAsync().Result);
				Console.WriteLine();

				response = client.GetAsync(baseAddress + "api/BadAsync").Result;

				Console.WriteLine(response);
				Console.WriteLine(response.Content.ReadAsStringAsync().Result);

				Console.ReadLine();
			}
		}

		private static async Task Worker(HttpClient client)
		{
			while (true)
			{
				var content = await client.PostAsync(_baseAddress + "api/Home/GetTask", new StringContent(""));
				var task = await content.Content.ReadAsAsync<QueueTask>();

				if (null == task)
				{
					await Task.Delay(1000);
					continue;
				} 

				await 
			}
		}
	}
}