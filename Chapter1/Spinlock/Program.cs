using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Spinlock
{
	/// <summary>
	/// Chapter 1 spinlock sample.
	/// </summary>
	/// <remarks>
	/// Core i7 2600 output (x64, Release):
	///   Lock: 1906ms
	///   Spinlock with memory barrier: 1761ms
	///   Spinlock without memory barrier: 1731ms
	/// </remarks>
	static class Program
	{
		private const int _count = 10000000;

		static void Main()
		{
			// Warm up
			var map = new Dictionary<double, double>();
			var r = Math.Sin(0.01);

			// lock
			map.Clear();
			var prm = 0d;
			var lockFlag = new object();
			var sw = Stopwatch.StartNew();
			for (int i = 0; i < _count; i++)
				lock (lockFlag)
				{
					map.Add(prm, Math.Sin(prm));
					prm += 0.01;
				}
			sw.Stop();
			Console.WriteLine("Lock: {0}ms", sw.ElapsedMilliseconds);

			// spinlock with memory barrier
			map.Clear();
			var spinLock = new SpinLock();
			prm = 0;
			sw = Stopwatch.StartNew();
			for (int i = 0; i < _count; i++)
			{
				var gotLock = false;
				try
				{
					spinLock.Enter(ref gotLock);
					map.Add(prm, Math.Sin(prm));
					prm += 0.01;
				}
				finally
				{
					if (gotLock)
						spinLock.Exit(true);
				}
			}
			sw.Stop();
			Console.WriteLine("Spinlock with memory barrier: {0}ms", sw.ElapsedMilliseconds);

			// spinlock without memory barrier
			map.Clear();
			prm = 0;
			sw = Stopwatch.StartNew();
			for (int i = 0; i < _count; i++)
			{
				var gotLock = false;
				try
				{
					spinLock.Enter(ref gotLock);
					map.Add(prm, Math.Sin(prm));
					prm += 0.01;
				}
				finally
				{
					if (gotLock)
						spinLock.Exit(false);
				}
			}
			sw.Stop();
			Console.WriteLine("Spinlock without memory barrier: {0}ms", sw.ElapsedMilliseconds);
		}
	}
}
