using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Tasks;


namespace Chapter6.Samples.NonConcurrentCollections
{
    [Task(processCount: 1, platform: BenchmarkPlatform.HostPlatform, targetIterationCount: 5, warmupIterationCount: 1,
        mode: BenchmarkMode.SingleRun)]
    public class SillyForeachBenchmark
    {
        private const int NumberOfElements = 42000;

        [Benchmark]
        public void RunWithLocks()
        {
            object syncRoot = new object();
            var source = Enumerable.Range(1, NumberOfElements).ToList();
            var destination = new List<int>();

            Parallel.ForEach(source,
                n =>
                {
                    lock (syncRoot)
                    {
                        destination.Add(n);
                    }
                });
        }
        

        [Benchmark]
        public void RunWithOneThread()
        {
            object syncRoot = new object();
            var source = Enumerable.Range(1, NumberOfElements).ToList();
            var destination = new List<int>();
            foreach (var n in source)
            {
                destination.Add(n);
            }
        }
    }
}