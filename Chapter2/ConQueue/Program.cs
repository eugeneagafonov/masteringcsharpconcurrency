using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		private const int _iterations = 1000000;
		private const int _writeThreads = 8;

		public static void Main()
		{
		    Measure(new QueueWrapper<string>());
		    Measure(new LockFreeQueueWrapper<string>());
		    Measure(new ConcurrentQueueWrapper<string>());

			Console.WriteLine("Queue: {0}ms", Measure(new QueueWrapper<string>()));
			Console.WriteLine("LockFreeQueue: {0}ms", Measure(new LockFreeQueueWrapper<string>()));
			Console.WriteLine("ConcurrentQueue: {0}ms", Measure(new ConcurrentQueueWrapper<string>()));
		}

		private static long Measure(IConcurrentQueue<string> queue)
		{
			var threads =
				Enumerable
					.Range(0, _writeThreads)
					.Select(
						n =>
							new Thread(
								() =>
								{
									for (var i = 0; i < _iterations; i++)
									{
										queue.Enqueue(i.ToString());
										Thread.SpinWait(100);
									}
								}))
					.Concat(
						new[]{new Thread(() =>
						{
							var left = _iterations*_writeThreads;
							while (left > 0)
							{
								string res;
								if (queue.TryDequeue(out res))
									left--;
							}
						})})
					.ToArray();
			var sw = Stopwatch.StartNew();
			foreach (var thread in threads)
				thread.Start();
			foreach (var thread in threads)
				thread.Join();
			sw.Stop();
			if (!queue.IsEmpty)
				throw new ApplicationException("Queue is not empty!");
			return sw.ElapsedMilliseconds;
		}

		class QueueWrapper<T> : IConcurrentQueue<T>
		{
			private readonly object _syncRoot = new object();
			private readonly Queue<T> _queue = new Queue<T>();


			public void Enqueue(T data)
			{
				lock (_syncRoot)
					_queue.Enqueue(data);
			}

			public bool TryDequeue(out T data)
			{
				if (_queue.Count > 0)
				{
					lock (_syncRoot)
					{
						if (_queue.Count > 0)
						{
							data = _queue.Dequeue();
							return true;
						}
					}
				}
				data = default(T);
				return false;
			}

			public bool IsEmpty
			{
				get { return _queue.Count == 0; }
			}
		}

		class LockFreeQueueWrapper<T> : LockFreeQueue<T>, IConcurrentQueue<T>
		{
		}

		class ConcurrentQueueWrapper<T> : ConcurrentQueue<T>, IConcurrentQueue<T>
		{
		}
	}


}
