using System.Web.Http;
using Owin;

namespace IISSynchronizationContext
{
	public class Startup
	{
		public void Configuration(IAppBuilder appBuilder)
		{
			var config = new HttpConfiguration();
			config.Routes.MapHttpRoute(
				"DefaultApi", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional}
				);

			appBuilder.UseWebApi(config);
		}
	}
}