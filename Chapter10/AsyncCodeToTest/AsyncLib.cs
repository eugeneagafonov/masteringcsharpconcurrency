using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncCodeToTest
{
	public class AsyncLib
	{
		public async Task GoodMethodAsync()
		{
			await Task.Delay(TimeSpan.FromSeconds(1));
		}

		public async Task DeadlockMethodAsync()
		{
			var lock1 = new object();
			var lock2 = new object();

			Task task1 = Task.Run(() =>
			{
				lock (lock1)
				{
					Thread.Sleep(200);
					lock (lock2)
					{
					}
				}
			});

			Task task2 = Task.Run(() =>
			{
				lock (lock2)
				{
					Thread.Sleep(200);
					lock (lock1)
					{
					}
				}
			});

			await Task.WhenAll(task1, task2);
		}
	}
}
