using System;
using System.Threading.Tasks;
using AsyncCodeToTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsyncUnitTests
{
	[TestClass]
	public class LockTests
	{
		[TestMethod]
		public async Task TestGoodAsync()
		{
			var lib = new AsyncLib();
			await lib.GoodMethodAsync().TimeoutAfter(2000);
		}


		[TestMethod]
		public async Task TestDeadlockAsync()
		{
			var lib = new AsyncLib();
			await lib.DeadlockMethodAsync().TimeoutAfter(2000);
		}
	}
}
