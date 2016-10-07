using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrencyBook.Samples
{
	static class Program
	{
		static void Main()
		{
			Task
				.Factory
				.StartNew(
					() =>
					{
						Console.WriteLine("Parent started");
						Task
							.Factory
							.StartNew(
								() =>
								{
									Console.WriteLine("Child started");
									Thread.Sleep(100);
									Console.WriteLine("Child complete");
								},
								TaskCreationOptions.AttachedToParent);
					})
				.Wait();
			Console.WriteLine("Parent complete");
		}
	}
}
