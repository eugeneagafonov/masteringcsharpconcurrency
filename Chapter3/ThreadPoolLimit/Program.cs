using System;
using System.Threading;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		private const int _threadCount = 256;
		private static int _runCount;

		static void Main()
		{
			int maxWorker, maxIO;
			ThreadPool.GetAvailableThreads(out maxWorker, out maxIO);
			Console.WriteLine("Max worker threads {0}", maxWorker);

			for (var i = 0; i < _threadCount; i++)
				ThreadPool.QueueUserWorkItem(
					s =>
					{
						Interlocked.Increment(ref _runCount);
						Thread.Sleep(5000);
						Interlocked.Decrement(ref _runCount);
					});
			Thread.Sleep(1000);
			while (_runCount > 0)
			{
				Console.WriteLine(_runCount);
				Thread.Sleep(100);
			}
		}
	}
}
