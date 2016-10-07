using System;
using System.Threading.Tasks;
using AsyncCodeToTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AsyncUnitTests
{
	[TestClass]
	public class CounterTests
	{
		[TestMethod]
		public async Task TestCounterWithRaceCondition()
		{
			var counter = new AsyncCounter();
			int count = await counter.CountWithRaceConditionAsync();
			Assert.AreEqual(0, count);
		}

		[TestMethod]
		public async Task TestCounterWitInterlocked()
		{
			var counter = new AsyncCounter();
			int count = await counter.CountWithInterlockedAsync();
			Assert.AreEqual(0, count);
		}
	}
}
