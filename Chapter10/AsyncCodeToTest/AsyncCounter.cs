using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeToTest
{
	public class AsyncCounter
	{
		public async Task<int> CountWithRaceConditionAsync()
		{
			const int iterations = 10000;
			var counter = 0;
			Action count =
				() =>
				{
					for (int i = 0; i < iterations; i++)
					{
						counter++;
						Thread.SpinWait(100);
						counter--;
					}
				};
			var tasks =
				Enumerable
					.Range(0, 8)
					.Select(n => Task.Run(count))
					.ToArray();

			await Task.WhenAll(tasks);

			return counter;
		}

		public async Task<int> CountWithInterlockedAsync()
		{
			const int iterations = 10000;
			var counter = 0;
			Action count =
				() =>
				{
					for (int i = 0; i < iterations; i++)
					{
						Interlocked.Increment(ref counter);
						Thread.SpinWait(100);
						Interlocked.Decrement(ref counter);
					}
				};
			var tasks =
				Enumerable
					.Range(0, 8)
					.Select(n => Task.Run(count))
					.ToArray();

			await Task.WhenAll(tasks);

			return counter;
		}
	}
}
