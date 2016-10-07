using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AsyncCodeToTest;
using AsyncServer;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsyncUnitTests
{
	[TestClass]
	public class ServerInMemoryTests
	{
		private static TestServer _server;
		private static HttpClient _client;

		[ClassInitialize]
		public static void ClassInit(TestContext context)
		{
			_server = TestServer.Create<Startup>();
			_client = _server.HttpClient;
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

		[ClassCleanup]
		public static void ClassCleanup()
		{
			_server.Dispose();
		}
	}
}
