using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		private static void Calc(int iterations)
		{
			var taskIds = new HashSet<int>();
			var sum = 0;
			Parallel.For(
				0,
				iterations,
				i =>
				{
					Thread.SpinWait(1000000);
					lock (taskIds)
						taskIds.Add(Task.CurrentId.Value);
				});
			Console.WriteLine("{0} iterations, {1} tasks", iterations, taskIds.Count);
		}

		static void Main()
		{
			Parallel.Invoke(
				() => Console.WriteLine("Action 1"),
				() =>
				{
					Thread.SpinWait(10000);
					Console.WriteLine("Action 2");
				},
				() => Console.WriteLine("Action 3"));
			Console.WriteLine("End");

			Calc(1);
			Calc(4);
			Calc(8);
			Calc(12);
			Calc(16);
			Calc(32);
			Calc(64);
		}
	}
}
