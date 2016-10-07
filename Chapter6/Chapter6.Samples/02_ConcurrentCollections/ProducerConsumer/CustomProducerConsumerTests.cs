using System;
using System.Threading;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections.ProducerConsumer
{
    [TestFixture]
    public class CustomProducerConsumerTests
    {
        [Test]
        public void Sample()
        {
            Action<string> processor = element =>
            {
                Console.WriteLine("[{0}]: Processing element '{1}'",
                    Thread.CurrentThread.ManagedThreadId, element);
            };

            var producerConcumer = new CustomProducerConsumer<string>(processor, Environment.ProcessorCount);
            for (int i = 0; i < 5; i++)
            {
                string item = "Item " + (i + 1);
                Console.WriteLine("[{0}]: Adding element '{1}'",
                    Thread.CurrentThread.ManagedThreadId, item);

                producerConcumer.Process("Item " + (i + 1));
            }

            Console.WriteLine("[{0}]: Complete adding new elements",
                Thread.CurrentThread.ManagedThreadId);

            producerConcumer.CompleteProcessing();
            // Dispose will block till all operations gets completed
            producerConcumer.Dispose();
        }
        
    }
}