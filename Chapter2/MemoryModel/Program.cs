using System;
using System.Threading;

namespace ConcurrencyBook.Samples
{
	class Program
	{
		bool _loop = true;

		static void Main()
		{
			var p = new Program();

			new Thread(() =>
			{
				Thread.Sleep(100);
				p._loop = false;
			})
			.Start();

			// run this in release build configuration with optimizations enabled
			while (p._loop) ;
			//// comment out the empty loop and uncomment this, and the loop will run normally
			//while (p._loop) { Console.Write(".");};

			Console.WriteLine("Exited the loop");
		}
	}
}
