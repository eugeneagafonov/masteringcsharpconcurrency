using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDebugParallelStacks
{
	class Program
	{
		static void Main(string[] args)
		{
			ParallelForEach().GetAwaiter().GetResult();
		}

		public static async Task ParallelForEach()
		{
			Parallel.ForEach(Enumerable.Range(0, 32), i =>
			{
				Console.WriteLine(i);
				if (i == 24)
				{
					Debugger.Break();
				}
				Thread.Sleep(new Random(i).Next(100, 500));
			});
		}
	}
}
