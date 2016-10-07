using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AsyncCodeToTest;

namespace AsyncServer
{
	public class HomeController : ApiController
	{
		[HttpGet]
		public int Sync()
		{
			var lib = new AsyncHttp();

			return lib.CountCharactersAsync(new Uri("http://google.com")).Result;
		}

		[HttpGet]
		public async Task<int> Async()
		{
			var lib = new AsyncHttp();

			return await lib.CountCharactersAsync(new Uri("http://google.com"));
		}
	}
}