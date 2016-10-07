using System;
using System.Net.Http;
using System.Threading.Tasks;
using AsyncCodeToTest;
using AsyncServer;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsyncUnitTests
{
	[TestClass]
	public class ServerHttpTests
	{
		private static HttpClient _client;

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			_client = new HttpClient();
			_client.BaseAddress = new Uri("http://localhost:1845/");
		}

		[TestMethod]
		public async Task TestSyncAction()
		{
			var response = await _client.GetAsync("/api/Home/Sync").TimeoutAfter(2000);
			var result = await response.Content.ReadAsAsync<int>();

			Assert.IsTrue(result > 0);
		}

		[TestMethod]
		public async Task TestAsyncAction()
		{
			var response = await _client.GetAsync("/api/Home/Async").TimeoutAfter(2000);
			var result = await response.Content.ReadAsAsync<int>();

			Assert.IsTrue(result > 0);
		}

		[ClassCleanup()]
		public static void ClassCleanup()
		{
			_client.Dispose();
		}
	}
}
