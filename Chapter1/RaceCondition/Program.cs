using System;
using System.Linq;
using System.Threading;

namespace RaceCondition
{
	static class Program
	{
		static void Main()
		{
			const int iterations = 10000;
			var counter = 0;
			ThreadStart proc =
				() =>
				{
					for (int i = 0; i < iterations; i++)
					{
						counter++;
						Thread.SpinWait(100);
						counter--;
					}
				};
			var threads =
				Enumerable
					.Range(0, 8)
					.Select(n => new Thread(proc))
					.ToArray();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			Console.WriteLine(counter);
		}
	}
}
