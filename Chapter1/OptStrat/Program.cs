using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace OptStrat
{
	/// <summary>
	/// Chapter 1 optimization strategy sample
	/// </summary>
	/// <remarks>
	/// Core i7 2600 output (x64, Release):
	///   Simple lock: 806ms
	///   Minimized lock: 321ms
	///   Minimized shared data: 165ms
	/// </remarks>
	static class Program
	{
		private const int _count = 1000000;
		private const int _threadCount = 8;

		private static readonly List<string> _result = new List<string>();

		private static string Calc(int prm)
		{
			Thread.SpinWait(100);
			return prm.ToString();
		}

		private static void SimpleLock(int from, int count)
		{
			for (var i = from; i < from + count; i++)
				lock (_result)
					_result.Add(Calc(i));
		}

		private static void MinimizedLock(int from, int count)
		{
			for (var i = from; i < from + count; i++)
			{
				var calc = Calc(i);
				lock (_result)
					_result.Add(calc);
			}
		}

		private static void MinimizedSharedData(int from, int count)
		{
			var tempRes = new List<string>(count);
			for (var i = from; i < from + count; i++)
			{
				var calc = Calc(i);
				tempRes.Add(calc);
			}
			lock (_result)
				_result.AddRange(tempRes);
		}

		private static long Measure(Func<int, ThreadStart> actionCreator)
		{
			_result.Clear();
			var threads =
				Enumerable
					.Range(0, _threadCount)
					.Select(n => new Thread(actionCreator(n)))
					.ToArray();
			var sw = Stopwatch.StartNew();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			sw.Stop();
			return sw.ElapsedMilliseconds;
		}

		static void Main()
		{
			// Warm up
			SimpleLock(1, 1);
			MinimizedLock(1, 1);
			MinimizedSharedData(1, 1);

			const int part = _count / _threadCount;

			var time = Measure(n => () => SimpleLock(n*part, part));
			Console.WriteLine("Simple lock: {0}ms", time);

			time = Measure(n => () => MinimizedLock(n * part, part));
			Console.WriteLine("Minimized lock: {0}ms", time);

			time = Measure(n => () => MinimizedSharedData(n * part, part));
			Console.WriteLine("Minimized shared data: {0}ms", time);
		}
	}
}
