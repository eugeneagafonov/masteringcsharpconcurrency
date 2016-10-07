using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncServer
{
	public class AsyncLib
	{
		public async Task<string> GoodMethodAsync()
		{
			await Task.Delay(TimeSpan.FromSeconds(2));
			return "Good async library method result";
		}

		public async Task<string> BadMethodAsync()
		{
			Thread.Sleep(TimeSpan.FromSeconds(2));
			return "Bad async library method result";
		}
	}
}