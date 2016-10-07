using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AsyncCodeToTest
{
	public class AsyncHttp
	{
		public async Task<int> CountCharactersAsync(Uri uri)
		{
			using (var client = new HttpClient())
			{
				var content = await client.GetStringAsync(uri);

				return content.Length;
			}
		}
	}
}
