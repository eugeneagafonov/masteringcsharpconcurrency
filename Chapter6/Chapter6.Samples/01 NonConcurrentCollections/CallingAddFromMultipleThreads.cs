using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples.NonConcurrentCollections
{
    [TestFixture]
    public class CallingAddFromMultipleThreads
    {
        [Test]
        public void Run_Add_Method_From_Parallel_Foreach()
        {
            var source = Enumerable.Range(1, 42000).ToList();
            var destination = new List<int>();
            
            Parallel.ForEach(source, n => destination.Add(n));

            Assert.AreEqual(source.Count, destination.Count);
        }

        [Test]
        public void Run_Add_Method_From_Parallel_Foreach_With_Lock()
        {
            object syncRoot = new object();
            var source = Enumerable.Range(1, 42000).ToList();
            var destination = new List<int>();

            Parallel.ForEach(source,
                n =>
                {
                    lock (syncRoot)
                    {
                        destination.Add(n);
                    }
                });

            Assert.AreEqual(source.Count, destination.Count);
        }
    }
}