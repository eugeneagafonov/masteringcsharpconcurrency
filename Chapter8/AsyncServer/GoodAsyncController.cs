using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace AsyncServer
{
	public class GoodAsyncController : ApiController
	{
		private readonly AsyncLib _client;

		public GoodAsyncController()
		{
			_client = new AsyncLib();
		}

		public async Task<HttpResponseMessage> Get()
		{
			var sw = Stopwatch.StartNew();
			string value = await _client.GoodMethodAsync();
			sw.Stop();
			var timespan = sw.Elapsed;
			return Request.CreateResponse(HttpStatusCode.OK,
				new { Message = value, Time = timespan });
		}
	}
}