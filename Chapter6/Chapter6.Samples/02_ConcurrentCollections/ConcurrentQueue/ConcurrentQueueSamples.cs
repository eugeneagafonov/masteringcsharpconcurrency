using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections.ConcurrentQueue
{
    [TestFixture]
    public class ConcurrentQueueSamples
    {
        [Test]
        public void BasicExample()
        {
        var queue = new ConcurrentQueue<string>();

        var task1 = Run(() =>
        {
            AddAndPrint(queue, "[T1]: Item 1");
            AddAndPrint(queue, "[T1]: Item 2");
            AddAndPrint(queue, "[T1]: Item 3");
            for (int n = 0; n < 74; n++)
            {
                queue.Enqueue("Temp");
            }

            Thread.Sleep(2000);
            TakeAndPrint(queue);
            TakeAndPrint(queue);
        }, threadName: "T1");
        
        var task2 = Run(() =>
        {
            AddAndPrint(queue, "[T2]: Item 1");
            AddAndPrint(queue, "[T2]: Item 2");
            AddAndPrint(queue, "[T2]: Item 3");

            Thread.Sleep(1000);
            TakeAndPrint(queue);
            TakeAndPrint(queue);
            TakeAndPrint(queue);
            TakeAndPrint(queue);
        }, threadName: "T2");

        Task.WaitAll(task1, task2);
        }

        private static Task Run(Action action, string threadName)
        {
            var tcs = new TaskCompletionSource<object>();
            var thread = new Thread(() =>
            {
                action();
                tcs.SetResult(null);
            });
            thread.Name = threadName;
            thread.Start();

            return tcs.Task;
        }

        private static void AddAndPrint(ConcurrentQueue<string> queue, string value)
        {
            Console.WriteLine("{0}: Add - {1}", Thread.CurrentThread.Name, value);
            queue.Enqueue(value);
        }

        private static void TakeAndPrint(ConcurrentQueue<string> queue)
        {
            string value;
            if (queue.TryDequeue(out value))
            {
                Console.WriteLine("{0}: Dequeue - {1}", Thread.CurrentThread.Name, value);
            }
        }
    }
}