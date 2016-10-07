using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace InterlockedInc
{
	static class Program
	{
		static void Main()
		{
			const int count = 1000000;

			// Locks
			var counterLock = new object();
			var counter = 0;
			ThreadStart proc =
				() =>
				{
					for (int i = 0; i < count; i++)
					{
						lock (counterLock)
							counter++;
						Thread.SpinWait(100);
						lock (counterLock)
							counter--;
					}
				};
			var threads =
				Enumerable
					.Range(0, 8)
					.Select(n => new Thread(proc))
					.ToArray();
			var sw = Stopwatch.StartNew();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			sw.Stop();
			Console.WriteLine("Locks: counter={0}, time = {1}ms", counter, sw.ElapsedMilliseconds);

			// Interlocked
			counter = 0;
			ThreadStart proc2 =
				() =>
				{
					for (int i = 0; i < count; i++)
					{
						Interlocked.Increment(ref counter);
						Thread.SpinWait(100);
						Interlocked.Decrement(ref counter);
					}
				};
			threads =
				Enumerable
					.Range(0, 8)
					.Select(n => new Thread(proc2))
					.ToArray();
			sw = Stopwatch.StartNew();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			sw.Stop();
			Console.WriteLine("Lock free: counter={0}, time = {1}ms", counter, sw.ElapsedMilliseconds);
		}
	}
}
