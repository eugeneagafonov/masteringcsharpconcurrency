using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections
{
    [TestFixture]
    public class ConcurrentBagSamples
    {
        [Test]
        public void ConcurrentBagBehavior()
        {
            var bag = new ConcurrentBag<string>();

            var task1 = Run(() =>
            {
                AddAndPrint(bag, "[T1]: Item 1");
                AddAndPrint(bag, "[T1]: Item 2");
                AddAndPrint(bag, "[T1]: Item 3");

                Thread.Sleep(2000);
                TakeAndPrint(bag);
                TakeAndPrint(bag);
            }, threadName: "T1");

            var task2 = Run(() =>
            {
                AddAndPrint(bag, "[T2]: Item 1");
                AddAndPrint(bag, "[T2]: Item 2");
                AddAndPrint(bag, "[T2]: Item 3");

                Thread.Sleep(1000);
                TakeAndPrint(bag);
                TakeAndPrint(bag);
                TakeAndPrint(bag);
                TakeAndPrint(bag);
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

        private static void AddAndPrint(ConcurrentBag<string> bag, string value)
        {
            Console.WriteLine("{0}: Add - {1}", Thread.CurrentThread.Name, value);
            bag.Add(value);
        }

        private static void TakeAndPrint(ConcurrentBag<string> bag)
        {
            string value;
            if (bag.TryTake(out value))
            {
                Console.WriteLine("{0}: Take - {1}", Thread.CurrentThread.Name, value);
            }
        }
    }
}