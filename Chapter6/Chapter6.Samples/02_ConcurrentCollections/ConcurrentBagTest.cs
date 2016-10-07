using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections
{
    [TestFixture]
    public class ConcurrentBagTest
    {
        [Test]
        public void RunWithConcurrentBag()
        {
            // Make sure that Parallel.ForEach uses multiple threads actually!
            var source = Enumerable.Range(1, 100000).ToList();
            var destination = new ConcurrentBag<int>();
            ConcurrentDictionary<int, int> dict = new ConcurrentDictionary<int, int>();
            Parallel.ForEach(source,
                n =>
                {
                    dict.AddOrUpdate(Thread.CurrentThread.ManagedThreadId, 1, (i, i1) => 1);
                    destination.Add(n);
                    int result;
                    destination.TryTake(out result);
                });

            Console.WriteLine("Threads: " + string.Join(", ", dict.Keys));
            //var list = destination.ToList();
            //count += list.Count;
            //count += destination.Count;
        } 
    }
}