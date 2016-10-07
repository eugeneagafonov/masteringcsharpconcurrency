using System;
using System.Threading;

namespace Deadlock
{
	static class Program
	{
		static void Main()
		{
			const int count = 10000;

			var a = new object();
			var b = new object();
			var thread1 =
				new Thread(
					() =>
					{
						for (int i = 0; i < count; i++)
							lock (a)
								lock (b)
									Thread.SpinWait(100);
					});
			var thread2 =
				new Thread(
					() =>
					{
						for (int i = 0; i < count; i++)
							lock (b)
								lock (a)
									Thread.SpinWait(100);
					});
			thread1.Start();
			thread2.Start();
			thread1.Join();
			thread2.Join();
			Console.WriteLine("Done");
		}
	}
}
