using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Granularity
{
	static class Program
	{
		private const int _totalSize = 1000;
		private const int _sizeElementaryDelay = 1000000;

		static void Main()
		{
			var random = new Random();
			var taskSizes =
				Enumerable
					.Range(0, _totalSize)
					.Select(n => random.NextDouble())
					.ToArray();
			for (var workSize = 256; workSize > 0; workSize-=4)
			{
				
				var total = 0;
				var tasks = new List<Task>();
				var i = 0;
				while (total < _totalSize)
				{
					var currentSize = (int)(taskSizes[i]*workSize) + 1;
					tasks.Add(
						new Task(
							() =>
							{
								Thread.SpinWait(currentSize*_sizeElementaryDelay);
							}));
					i++;
					total += currentSize;
				}
				var sw = Stopwatch.StartNew();
				foreach (var task in tasks)
					task.Start();
				Task.WaitAll(tasks.ToArray());
				sw.Stop();
				Console.WriteLine(
					"Work size {0}, Task count {1}, Effectiveness {2:####} works/ms",
					workSize,
					tasks.Count,
					((double)total*_sizeElementaryDelay)/sw.ElapsedMilliseconds);
			}
		}
	}
}
