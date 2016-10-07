using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections
{
    [TestFixture]
    public class SynchronizedArrayList
    {
        [Test]
        public void Run_Add_Method_From_Parallel_Foreach_With_Synchronized_ArrayList()
        {
            var source = Enumerable.Range(1, 42000).ToList();
            var destination = ArrayList.Synchronized(new List<int>());

            Parallel.ForEach(source,
                n =>
                {
                    destination.Add(n);
                });

            Assert.AreEqual(source.Count, destination.Count);
        }

        [Test]
        public void Copy_In_Parallel_With_Partial_Locks()
        {
            var source = Enumerable.Range((short)1, short.MaxValue).ToList();
            var destination = new short[short.MaxValue];
            const int numberOfLocks = 4;
            
            var locks = 
                Enumerable.Range(1, numberOfLocks)
                .Select(n => new object()).ToList();

            Parallel.For(0, source.Count, 
                i =>
                {
                    //lock (locks[0])
                    {
                        destination[i] = (short)source[i];
                    }
                });

            CollectionAssert.AreEquivalent(source, destination);
        }
 
    }
}