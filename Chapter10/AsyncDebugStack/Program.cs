using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncDebugStack
{
	class Program
	{
		static void Main(string[] args)
		{
			StartAsyncOperation().GetAwaiter().GetResult();
		}

		public static async Task StartAsyncOperation()
		{
			Console.WriteLine("Starting async operation");
			await AsyncOperation();
			Console.WriteLine("After finishing async operation");
		}

		public static async Task AsyncOperation()
		{
			Console.WriteLine("Inside async operation");
			await Task.Delay(TimeSpan.FromSeconds(1));
			Debugger.Break();
			// here you can open call stack window and threads window
			// in visual studio
			Console.WriteLine("Async operation complete!");
		}
	}
}
