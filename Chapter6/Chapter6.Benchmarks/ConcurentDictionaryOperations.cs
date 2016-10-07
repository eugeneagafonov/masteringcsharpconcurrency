using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BenchmarkDotNet;
using BenchmarkDotNet.Tasks;

namespace Chapter6.Benchmarks
{
    [Task(processCount: 1, platform: BenchmarkPlatform.HostPlatform, targetIterationCount: 5, warmupIterationCount: 1,
        mode: BenchmarkMode.SingleRun)]
    public class ConcurentDictionaryOperations
    {
        private const int NumberOfElements = 10000000;
        public static ConcurrentDictionary<string, int> Create(int numberOfElements)
        {
            var result = new ConcurrentDictionary<string, int>(42, Enumerable.Empty<KeyValuePair<string, int>>(), EqualityComparer<string>.Default);
            var random = new Random(42);
            int cap = numberOfElements/42;
            for (int n = 0; n < numberOfElements; n++)
            {
                var next = random.Next(cap);
                result.AddOrUpdate(next.ToString(), 1, (s, i) => i + 1);
            }

            return result;
        }

        private static ConcurrentDictionary<string, int> Dictionary = Create(NumberOfElements);

        private const int NumberOfTasks = 4;

        [Benchmark]
        public void DictionaryCount()
        {
            Func<int> taskFunc =
                () =>
                {
                    return Dictionary.Count;
                };
            
            var tcs = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!tcs.IsCancellationRequested)
                {
                    Dictionary.AddOrUpdate("42", 1, (s, i) => i + 1);
                    Thread.Sleep(0);
                }
            });

            var tasks = Enumerable.Range(1, NumberOfTasks).Select(_ => Task.Run(taskFunc)).ToArray();
            
            Task.WaitAll(tasks);
            tcs.Cancel();
        }
        
        [Benchmark]
        public void EnumerableCount()
        {
            Func<int> taskFunc =
                () =>
                {
                    return Dictionary.Count();
                };

            var tcs = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!tcs.IsCancellationRequested)
                {
                    Dictionary.AddOrUpdate("42", 1, (s, i) => i + 1);
                    Thread.Sleep(0);
                }
            });

            var tasks = Enumerable.Range(1, NumberOfTasks).Select(_ => Task.Run(taskFunc)).ToArray();

            Task.WaitAll(tasks);
            tcs.Cancel();

        }
        
        [Benchmark]
        public void DictionaryIsEmpty()
        {
            Func<bool> taskFunc =
                () =>
                {
                    return Dictionary.IsEmpty;
                };

            var tcs = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!tcs.IsCancellationRequested)
                {
                    Dictionary.AddOrUpdate("42", 1, (s, i) => i + 1);
                    Thread.Sleep(0);
                }
            });

            var tasks = Enumerable.Range(1, NumberOfTasks).Select(_ => Task.Run(taskFunc)).ToArray();

            Task.WaitAll(tasks);
            tcs.Cancel();
        }
        
        [Benchmark]
        public void EnumerableIsEmpty()
        {
            Func<bool> taskFunc =
                () =>
                {
                    return !Dictionary.Any();
                };

            var tcs = new CancellationTokenSource();
            Task.Run(() =>
            {
                while (!tcs.IsCancellationRequested)
                {
                    Dictionary.AddOrUpdate("42", 1, (s, i) => i + 1);
                    Thread.Sleep(0);
                }
            });

            var tasks = Enumerable.Range(1, NumberOfTasks).Select(_ => Task.Run(taskFunc)).ToArray();

            Task.WaitAll(tasks);
            tcs.Cancel();
        }
    }
}