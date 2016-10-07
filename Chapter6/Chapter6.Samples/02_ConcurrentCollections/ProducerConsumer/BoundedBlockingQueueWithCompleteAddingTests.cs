using System;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections.ProducerConsumer.WithCompleteAdding
{
    [TestFixture]
    public class BoundedBlockingQueueWithCompleteAddingTests
    {
        [Test]
        public void Sample()
        {
var queue = new BoundedBlockingQueue<string>(3);

var t1 = Task.Run(() =>
{
    AddAndPrint(queue, "1");
    AddAndPrint(queue, "2");
    AddAndPrint(queue, "3");
    AddAndPrint(queue, "4");
    AddAndPrint(queue, "5");

    queue.CompleteAdding();
    Console.WriteLine("[{0}]: finished producing elements",
        Thread.CurrentThread.ManagedThreadId);

});

var t2 = Task.Run(() =>
{
    foreach (var element in queue.Consume())
    {
        Print(element);
    }
                
    Console.WriteLine("[{0}]: Processing finished.",
        Thread.CurrentThread.ManagedThreadId);
});

var t3 = Task.Run(() =>
{
    foreach (var element in queue.Consume())
    {
        Print(element);
    }
                
    Console.WriteLine("[{0}]: Processing finished.",
        Thread.CurrentThread.ManagedThreadId);
});

Task.WaitAll(t1, t2, t3);
}

        private void AddAndPrint<T>(BoundedBlockingQueue<T> queue, T value)
        {
            queue.Add(value);
            Console.WriteLine("[{0}]: Added {1}", Thread.CurrentThread.ManagedThreadId, value);
            //Thread.Sleep(100);
        }

        private void Print<T>(T element)
        {
            Console.WriteLine("[{0}]: Took {1}", Thread.CurrentThread.ManagedThreadId, element);
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