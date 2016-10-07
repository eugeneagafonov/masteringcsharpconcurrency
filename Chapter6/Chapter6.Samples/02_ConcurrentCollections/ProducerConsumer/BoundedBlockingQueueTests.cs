using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections.ProducerConsumer
{
    [TestFixture]
    public class BoundedBlockingQueueTests
    {
        [Test]
        public void Add_Method_Waits_On_Full_Queue()
        {
            var queue = new BoundedBlockingQueue<string>(3);

            var t1 = Task.Run(() =>
            {
                AddAndPrint(queue, "1");
                AddAndPrint(queue, "2");
                AddAndPrint(queue, "3");
                AddAndPrint(queue, "4");
            });

            var t2 = Task.Run(() =>
            {
                Console.WriteLine("Sleeping before taking an element.");
                Thread.Sleep(1000);
                Console.WriteLine("Consuming first element!");
                TakeAndPrint(queue);
            });

            Task.WaitAll(t1, t2);
        }
        
        [Test]
        public void Take_Method_Waits_On_Empty_Queue()
        {
            var queue = new BoundedBlockingQueue<string>(3);

            var t1 = Task.Run(() =>
            {
                Console.WriteLine("Waiting for an element.");
                TakeAndPrint(queue);
            });

            var t2 = Task.Run(() =>
            {
                Console.WriteLine("Sleeping before adding an element.");
                Thread.Sleep(1000);
                Console.WriteLine("Ready to add new element!");
                AddAndPrint(queue, "42");
            });

            Task.WaitAll(t1, t2);
        }

        private void AddAndPrint<T>(BoundedBlockingQueue<T> queue, T value)
        {
            Console.WriteLine("Adding {0}", value);
            queue.Add(value);
            Console.WriteLine("Added {0}", value);
        }

        private T TakeAndPrint<T>(BoundedBlockingQueue<T> queue)
        {
            Console.WriteLine("Taking element...");
            T result = queue.Take();
            Console.WriteLine("Took element {0}", result);
            return result;
        }
    }
}