using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		private const int _iterations = 1000;
		private const int _iterationDepth = 100;
		private const int _threadCount = 8;

		private static long Measure(StackBase<string> stack)
		{
			var threads =
				Enumerable
					.Range(0, _threadCount)
					.Select(
						n =>
							new Thread(
								() =>
								{
									for (int j = 0; j < _iterations; j++)
									{
										for (int i = 0; i < _iterationDepth; i++)
											stack.Push(i.ToString());
										string res;
										for (int i = 0; i < _iterationDepth; i++)
											stack.TryPop(out res);
									}
								}))
					.ToArray();
			var sw = Stopwatch.StartNew();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			sw.Stop();
			if (!stack.IsEmpty)
				throw new ApplicationException("Stack must be empty!");
			return sw.ElapsedMilliseconds;
		}

		static void Main()
		{
			Console.WriteLine("LockStack: {0}ms", Measure(new LockStack<string>()));
			Console.WriteLine("LockFreeStack: {0}ms", Measure(new LockFreeStack<string>()));
			Console.WriteLine("MonitorStack: {0}ms", Measure(new MonitorStack<string>()));
			Console.WriteLine("ConcurrentStack: {0}ms", Measure(new ConcurrentStackWrapper<string>()));
		}
	}
}
