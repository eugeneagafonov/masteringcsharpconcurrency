using System;
using System.Threading;

namespace Monitor_
{
	class Program
	{
		static void Main()
		{
			var arg = 0;
			var result = "";
			var counter = 0;
			var lockHandle = new object();
			var calcThread =
				new Thread(
					() =>
					{
						while (true)
							lock (lockHandle)
							{
								counter++;
								result = arg.ToString();
								Monitor.Pulse(lockHandle);
								Monitor.Wait(lockHandle);
							}
					})
				{
					IsBackground = true
				};
			lock (lockHandle)
			{
				calcThread.Start();
				Thread.Sleep(100);
				Console.WriteLine("counter = {0}, result = {1}", counter, result);

				arg = 123;
				Monitor.Pulse(lockHandle);
				Monitor.Wait(lockHandle);
				Console.WriteLine("counter = {0}, result = {1}", counter, result);

				arg = 321;
				Monitor.Pulse(lockHandle);
				Monitor.Wait(lockHandle);
				Console.WriteLine("counter = {0}, result = {1}", counter, result);
			}
		}
	}
}
