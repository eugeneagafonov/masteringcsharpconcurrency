using System;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		//private static TaskAwaiter<string> GetAwaiter(this Uri url)
		//{
		//	return new HttpClient().GetStringAsync(url).GetAwaiter();
		//}

		private static async Task Do()
		{
			var content = await new Uri("http://google.com");
			Console.WriteLine(content.Substring(0, 10));
		}

		struct DownloadAwaiter : INotifyCompletion
		{
			private readonly TaskAwaiter<string> _awaiter;

			public DownloadAwaiter(Uri uri)
			{
				Console.WriteLine("Start downloading from {0}", uri);
				var task = new HttpClient().GetStringAsync(uri);
				_awaiter = task.GetAwaiter();
				task
					.GetAwaiter()
					.OnCompleted(() => Console.WriteLine("download completed"));
			}

			public bool IsCompleted
			{
				get { return _awaiter.IsCompleted; }
			}

			public void OnCompleted(Action continuation)
			{
				_awaiter.OnCompleted(continuation);
			}

			public string GetResult()
			{
				return _awaiter.GetResult();
			}
		}

		private static DownloadAwaiter GetAwaiter(this Uri uri)
		{
			return new DownloadAwaiter(uri);
		}

		static void Main()
		{
			Do();
			Console.ReadLine();
		}
	}
}
