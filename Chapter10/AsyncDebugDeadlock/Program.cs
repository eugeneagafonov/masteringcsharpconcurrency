using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncDebugDeadlock
{
	class Program
	{
		static void Main(string[] args)
		{
			DeadlockMethodAsync().GetAwaiter().GetResult();
		}

		public static async Task DeadlockMethodAsync()
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
					Debugger.Break();
					lock (lock1)
					{
					}
				}
			});

			Debugger.Break();
			// here you can open Tasks window in Visual Studio
			await Task.WhenAll(task1, task2);
		}

	}
}
