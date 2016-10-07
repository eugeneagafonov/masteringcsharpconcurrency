using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Tasks;

namespace Chapter6.Benchmarks
{
    [Task(processCount: 1, platform: BenchmarkPlatform.HostPlatform, targetIterationCount: 5, warmupIterationCount: 1,
        mode: BenchmarkMode.SingleRun)]
    public class ConcurrentBagAndOthersWithStrings
    {
        private const int NumberOfElements = 420000;

        // ConcurentBag - sucks!
        [Benchmark]
        public void RunWithConcurrentBag()
        {
            var source = Enumerable.Range(1, NumberOfElements).Select(n => n.ToString()).ToList();
            var destination = new ConcurrentBag<string>();

            Parallel.For(0, source.Count,
                n =>
                {

                    destination.Add(source[n]);

                    if (n > 100)
                    {
                        string result;
                        destination.TryTake(out result);
                    }
                });

            //var list = destination.ToList();
            //count += list.Count;
            count += destination.Count;
        }

        [Benchmark]
        public void RunWithConcurrentBagThatRemovesAllStuff()
        {
            var source = Enumerable.Range(1, NumberOfElements).Select(n => n.ToString()).ToList();
            var destination = new ConcurrentBag<string>();

            Parallel.For(0, source.Count,
                n =>
                {

                    destination.Add(source[n]);

                    {
                        string result;
                        destination.TryTake(out result);
                    }
                });

            //var list = destination.ToList();
            //count += list.Count;
            count += destination.Count;
        }

        public static int count;

        [Benchmark]
        public void RunWithConcurrentQueue()
        {
            var source = Enumerable.Range(1, NumberOfElements).Select(n => n.ToString()).ToList();
            var destination = new ConcurrentQueue<string>();

            Parallel.For(0, source.Count,
                n =>
                {
                    destination.Enqueue(source[n]);
                    if (n > 100)
                    {
                        string result;
                        destination.TryDequeue(out result);
                    }
                });

            //var list = destination.ToList();
            //count += list.Count;
            count += destination.Count;
        }

        [Benchmark]
        public void RunWithLocks()
        {
            object syncRoot = new object();
            var source = Enumerable.Range(1, NumberOfElements).Select(n => n.ToString()).ToList();
            var destination = new List<string>();

            Parallel.For(0, source.Count,
                n =>
                {
                    lock (syncRoot)
                    {
                        destination.Add(source[n]);
                        if (n > 100)
                        {
                            destination.Remove(source[n]);
                        }
                    }
                });

            //var list = destination.ToList();
            //count += list.Count;
            count += destination.Count;
        }

    }
}