using System;
using System.Threading;

namespace MaxThreads
{
	static class Program
	{
		static void Main()
		{
			Console.WriteLine(IntPtr.Size);

			var cnt = 0;
			try
			{
				for (var i = 0; i < int.MaxValue; i++)
				{
					new Thread(() => Thread.Sleep(Timeout.Infinite)).Start();
					cnt++;
				}
			}
			catch
			{
				Console.WriteLine(cnt);
			}
		}
	}
}
