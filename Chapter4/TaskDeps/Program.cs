using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskDeps
{
	static class Program
	{
		static void Main()
		{
			// Sample 1
			//var taskA =
			//	new Task<string>(
			//		() =>
			//		{
			//			Console.WriteLine("Task A started");
			//			Thread.Sleep(1000);
			//			Console.WriteLine("Task A complete");
			//			return "A";
			//		});
			//taskA.Start();
			//var taskB =
			//	new Task(
			//		() =>
			//		{
			//			Console.WriteLine("Task B started");
			//			Console.WriteLine("Task A result is {0}", taskA.Result);
			//		});
			//taskB.Start();
			//taskB.Wait();

			// Sample 2
			//var taskA =
			//	new Task<string>(
			//		() =>
			//		{
			//			Console.WriteLine("Task A started");
			//			Thread.Sleep(1000);
			//			Console.WriteLine("Task A complete");
			//			return "A";
			//		});
			//taskA
			//	.ContinueWith(
			//		task =>
			//		{
			//			Console.WriteLine("Task B started");
			//			Console.WriteLine("Task A result is {0}", task.Result);
			//		});
			//taskA.Start();
			//taskA.Wait();

			// Sample 3
			//var taskA =
			//	new Task<string>(
			//		() =>
			//		{
			//			Console.WriteLine("Task A started");
			//			Thread.Sleep(1000);
			//			Console.WriteLine("Task A complete");
			//			return "A";
			//		});
			//taskA.Start();

			//var taskB1 = new Task(() => Console.WriteLine("Task B1 started"));
			//taskB1.Start();

			//taskA.ContinueWith(task => Console.WriteLine("Task A result is {0}", task.Result));

			//taskA.Wait();

			// Sample 4
			var taskA =
				new Task<string>(
					() =>
					{
						Console.WriteLine("Task A started");
						Thread.Sleep(1000);
						Console.WriteLine("Task A complete");
						return "A";
					});
			taskA.Start();

			var taskB1 =
				new Task<string>(
					() =>
					{
						Console.WriteLine("Task B1 started");
						Thread.Sleep(500);
						Console.WriteLine("Task B1 complete");
						return "B";
					});
			taskB1.Start();

			Task
				.Factory
				.ContinueWhenAll(
					new []{taskA, taskB1},
					tasks => Console.WriteLine("Task A result is {0}, Task B result is {1}", tasks[0].Result, tasks[1].Result));

			taskA.Wait();
		}
	}
}
