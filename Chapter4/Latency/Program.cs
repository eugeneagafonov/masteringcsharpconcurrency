using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyBook.Sample
{
	static class Program
	{
		private const int _measureCount = 100000;

		static void Main()
		{
			for (var longThreadCount = 0; longThreadCount < 24; longThreadCount++)
			{
				// Create coarse grained tasks
				var longThreads = new List<Task>();
				for (var i = 0; i < longThreadCount; i++)
					longThreads.Add(
						Task.Factory.StartNew(
							() => Thread.Sleep(1000),
							TaskCreationOptions.LongRunning));

				// Measure latency
				var sw = Stopwatch.StartNew();
				for (var i = 0; i < _measureCount; i++)
					Task
						.Factory
						.StartNew(() => Thread.SpinWait(100))
						.Wait();
				sw.Stop();
				Console.WriteLine(
					"Long running threads {0}. Average latency {1:0.###} ms",
					longThreadCount,
					(double)sw.ElapsedMilliseconds / _measureCount);

				Task.WaitAll(longThreads.ToArray());
			}
		}
	}
}
