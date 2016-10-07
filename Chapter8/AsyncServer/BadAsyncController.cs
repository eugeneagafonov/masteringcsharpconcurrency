using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncServer
{
	public class BadAsyncController : ApiController
	{
		private readonly AsyncLib _client;

		public BadAsyncController()
		{
			_client = new AsyncLib();
		}

		public async Task<HttpResponseMessage> Get()
		{
			var sw = Stopwatch.StartNew();
			string value = await _client.BadMethodAsync();
			sw.Stop();
			var timespan = sw.Elapsed;
			return Request.CreateResponse(HttpStatusCode.OK,
				new { Message = value, Time = timespan });
		}
	}
}