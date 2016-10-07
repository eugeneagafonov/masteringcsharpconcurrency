using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;

namespace IISSynchronizationContext
{
	public class HomeController : ApiController
	{
		[HttpGet]
		public int Sync()
		{
			var lib = new AsyncLib();

			return lib.CountCharactersAsync(new Uri("http://google.com")).Result;
		}

		[HttpGet]
		public async Task<int> Async()
		{
			var lib = new AsyncLib();

			return await lib.CountCharactersAsync(new Uri("http://google.com"));
		}
	}
}