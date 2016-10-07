using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		static void Main()
		{
			//var cancelSource = new CancellationTokenSource();
			//var token = cancelSource.Token;
			//var task =
			//	Task
			//		.Factory
			//		.StartNew(
			//			() =>
			//			{
			//				while (true)
			//					token.ThrowIfCancellationRequested();
			//			},
			//			token);
			//while (task.Status != TaskStatus.Running) {}
			//cancelSource.Cancel();
			//while (!task.IsCompleted) {}
			////task.Wait();
			//Console.WriteLine("Status = {0}, IsCanceled = {1}", task.Status, task.IsCanceled);
			//Console.WriteLine(task.Exception);

			//Task
			//	.Factory
			//	.StartNew(
			//		() =>
			//		{
			//			Task
			//				.Factory
			//				.StartNew(
			//					() => { throw new ApplicationException("Test exception"); },
			//					TaskCreationOptions.AttachedToParent);
			//		})
			//	.Wait();

			//var t = Task
			//	.Factory
			//	.StartNew(
			//		() =>
			//		{
			//			Task
			//				.Factory
			//				.StartNew(
			//					() => { throw new ApplicationException("Test exception"); },
			//					TaskCreationOptions.AttachedToParent);
			//		});
			//try
			//{
			//	t.Wait();
			//}
			//catch (AggregateException ae)
			//{
			//	foreach (Exception e in ae.InnerExceptions)
			//	{
			//		Console.WriteLine("{0}: {1}", e.GetType(), e.Message);
			//	}
			//}

			var t = Task.Factory.StartNew(
					() =>
					{
						Task.Factory.StartNew(
								() =>
								{
									Task.Factory.StartNew(
											() =>
											{
												throw new ApplicationException("And we need to go deeper");
											},
											TaskCreationOptions.AttachedToParent);
									throw new ApplicationException("Test exception");
								},
								TaskCreationOptions.AttachedToParent);
						Task.Factory.StartNew(
								() =>
								{
									throw new ApplicationException("Test sibling exception");
								},
								TaskCreationOptions.AttachedToParent);
					});
			try
			{
				t.Wait();
			}
			catch (AggregateException ae)
			{
				foreach (Exception e in ae.Flatten().InnerExceptions)
				{
					Console.WriteLine("{0}: {1}", e.GetType(), e.Message);
				}
			}
		}
	}
}
