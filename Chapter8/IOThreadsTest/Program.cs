using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace IOThreadsTest
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			var tasks = new List<Task<string>>();

			for (var i = 0; i < 100; i++)
			{
				tasks.Add(Task.Run(() =>
				{
					Thread.Sleep(5000);
					return IssueHttpRequest();
				}));
			}

			var allComplete = Task.WhenAll(tasks);

			while (allComplete.Status != TaskStatus.RanToCompletion)
			{
				Thread.Sleep(1000);
				PrintThreadCounts();
			}

			Console.WriteLine(tasks[0].Result.Substring(0, 160));
		}

		private static async Task<string> IssueHttpRequest()
		{
			var str = await new HttpClient().GetStringAsync("http://google.com");
//			Thread.Sleep(5000);
			return str;
		}

		private static void PrintThreadCounts()
		{
			int ioThreads;
			int maxIoThreads;
			int workerThreads;
			int maxWorkerThreads;

			ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxIoThreads);
			ThreadPool.GetAvailableThreads(out workerThreads, out ioThreads);

			Console.WriteLine(
				"Worker threads: {0}, I/O threads: {1}, Total threads: {2}",
				maxWorkerThreads - workerThreads,
				maxIoThreads - ioThreads,
				Process.GetCurrentProcess().Threads.Count
				);
		}
	}
}