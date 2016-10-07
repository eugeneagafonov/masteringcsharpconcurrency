using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace RWLocks
{
	/// <summary>
	/// Chapter 1 reader writer lock pattern sample.
	/// </summary>
	/// <remarks>
	/// Core i7-2600 output (x64, Release):
	///   Simple lock: 367ms
	///   ReaderWriterLock: 246ms
	///   ReaderWriterLockSlim: 183ms
	/// </remarks>
	static class Program
	{
		private const int _count = 100000;
		private const int _readersCount = 5;
		private const int _writersCount = 1;
		private const int _readPayload = 100;
		private const int _writePayload = 100;

		#region Tests common
		private static readonly Dictionary<int, string> _map = new Dictionary<int, string>();

		private static void ReaderProc()
		{
			string val;
			_map.TryGetValue(Environment.TickCount%_count, out val);
			// Do some work
			Thread.SpinWait(_readPayload);
		}

		private static void WriterProc()
		{
			var n = Environment.TickCount%_count;
			// Do some work
			Thread.SpinWait(_writePayload);
			_map[n] = n.ToString();
		}

		private static long Measure(Action reader, Action writer)
		{
			var threads =
				Enumerable
					.Range(0, _readersCount)
					.Select(
						n => new Thread(
							() =>
							{
								for (int i = 0; i < _count; i++)
									reader();
							}))
					.Concat(
						Enumerable
							.Range(0, _writersCount)
							.Select(
								n => new Thread(
									() =>
									{
										for (int i = 0; i < _count; i++)
											writer();
									})))
					.ToArray();
			_map.Clear();
			var sw = Stopwatch.StartNew();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			sw.Stop();
			return sw.ElapsedMilliseconds;
		}
		#endregion

		#region Simple lock
		private static readonly object _simpleLockLock = new object();

		private static void SimpleLockReader()
		{
			lock (_simpleLockLock)
				ReaderProc();
		}

		private static void SimpleLockWriter()
		{
			lock (_simpleLockLock)
				WriterProc();
		}
		#endregion

		#region ReaderWriterLock
		private static readonly ReaderWriterLock _rwLock = new ReaderWriterLock();

		private static void RWLockReader()
		{
			_rwLock.AcquireReaderLock(-1);
			try
			{
				ReaderProc();
			}
			finally
			{
				_rwLock.ReleaseReaderLock();
			}
		}

		private static void RWLockWriter()
		{
			_rwLock.AcquireWriterLock(-1);
			try
			{
				WriterProc();
			}
			finally
			{
				_rwLock.ReleaseWriterLock();
			}
		}
		#endregion

		#region ReaderWriterLockSlim
		private static readonly ReaderWriterLockSlim _rwLockSlim = new ReaderWriterLockSlim();

		private static void RWLockSlimReader()
		{
			_rwLockSlim.EnterReadLock();
			try
			{
				ReaderProc();
			}
			finally
			{
				_rwLockSlim.ExitReadLock();
			}
		}

		private static void RWLockSlimWriter()
		{
			_rwLockSlim.EnterWriteLock();
			try
			{
				WriterProc();
			}
			finally
			{
				_rwLockSlim.ExitWriteLock();
			}
		}
		#endregion

		static void Main()
		{
			// Warm up
			Measure(SimpleLockReader, SimpleLockWriter);
			// Measure
			var simpleLockTime = Measure(SimpleLockReader, SimpleLockWriter);
			Console.WriteLine("Simple lock: {0}ms", simpleLockTime);

			// Warm up
			Measure(RWLockReader, RWLockWriter);
			// Measure
			var rwLockTime = Measure(RWLockReader, RWLockWriter);
			Console.WriteLine("ReaderWriterLock: {0}ms", rwLockTime);

			// Warm up
			Measure(RWLockSlimReader, RWLockSlimWriter);
			// Measure
			var rwLockSlimTime = Measure(RWLockSlimReader, RWLockSlimWriter);
			Console.WriteLine("ReaderWriterLockSlim: {0}ms", rwLockSlimTime);
		}
	}
}
