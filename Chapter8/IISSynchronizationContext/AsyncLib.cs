using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IISSynchronizationContext
{
	public class AsyncLib
	{
		public async Task<int> CountCharactersAsync(Uri uri)
		{
			using (var client = new HttpClient())
			{
				var content = await client.GetStringAsync(uri)
					.ConfigureAwait(continueOnCapturedContext: false)
				;

				return content.Length;
			}
		}
	}
}