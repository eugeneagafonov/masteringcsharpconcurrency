using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ParallelPipeline
{
    class Program
    {
        private const int ParallelismDegree = 4;
        private const int Count = 1;

        static void Main(string[] args)
        {
            var cts = new CancellationTokenSource();

            Task.Run(() =>
            {
                if (Console.ReadKey().KeyChar == 'c')
                {
                    cts.Cancel();
                }
            });

            var sourceArrays = new BlockingCollection<string>[ParallelismDegree];
            for (int i = 0; i < sourceArrays.Length; i++)
            {
                sourceArrays[i] = new BlockingCollection<string>(Count);
            }

            var getWeatherStep = new PipelineWorkerAsync<string, Weather>
            (
                sourceArrays,
                city => WeatherService.GetWeatherAsync(city),
                cts.Token,
                "Get Weather",
                Count
            );

            var convertTempStep = new PipelineWorkerAsync<Weather, Tuple<string, decimal>>
            (
                getWeatherStep.Output,
                weather => Task.FromResult(Tuple.Create(weather.City, weather.TemperatureCelcius * (decimal)9/5 + 32)),
                cts.Token,
                "Convert Temperature",
                Count
                );

            var printInfoStep = new PipelineWorkerAsync<Tuple<string, decimal>, string>
            (
                convertTempStep.Output,
                t => Console.WriteLine("The temperature in {0} is {1}F on thread id {2}",
                    t.Item1, t.Item2, Thread.CurrentThread.ManagedThreadId),
                cts.Token,
                "Print Information"
                );

            try
            {
                Parallel.Invoke(
                () =>
                {
                    Parallel.ForEach(
                        new[] {"Seattle", "New York", "Los Angeles", "San Francisco"},
                        (city, state) =>
                        {
                            if (cts.Token.IsCancellationRequested)
                            {
                                state.Stop();
                            }

                            AddCityToSourceCollection(sourceArrays, city, cts.Token);
                        });
                        foreach (var arr in sourceArrays)
                        {
                            arr.CompleteAdding();
                        }
                    },
                    () => getWeatherStep.RunAsync().GetAwaiter().GetResult(),
                    () => convertTempStep.RunAsync().GetAwaiter().GetResult(),
                    () => printInfoStep.RunAsync().GetAwaiter().GetResult()
                );
            }
            catch (AggregateException ae)
            {
                foreach (var ex in ae.InnerExceptions)
                    Console.WriteLine(ex.Message + ex.StackTrace);
            }

            if (cts.Token.IsCancellationRequested)
            {
                Console.WriteLine("Operation has been canceled! Press ENTER to exit.");
            }
            else
            {
                Console.WriteLine("Press ENTER to exit.");
            }
            Console.ReadLine();
        }

        static void AddCityToSourceCollection(BlockingCollection<string>[] cities, string city, CancellationToken token)
        {
            BlockingCollection<string>.TryAddToAny(cities, city, 50, token);
            Console.WriteLine("Added {0} to fetch weather on thread id {1}", city, Thread.CurrentThread.ManagedThreadId);
            Thread.Sleep(TimeSpan.FromMilliseconds(100));
        }

        class PipelineWorkerAsync<TInput, TOutput>
        {
            Func<TInput, Task<TOutput>> _processorAsync = null;
            Action<TInput> _outputProcessor = null;
            BlockingCollection<TInput>[] _input;
            CancellationToken _token;
            private int _count;

            public PipelineWorkerAsync(
                    BlockingCollection<TInput>[] input,
                    Func<TInput, Task<TOutput>> processorAsync,
                    CancellationToken token,
                    string name,
                    int count)
            {
                _input = input;
                _count = count;
                _processorAsync = processorAsync;
                _token = token;

                Output = new BlockingCollection<TOutput>[_input.Length];
                for (int i = 0; i < Output.Length; i++)
                    Output[i] = null == input[i] ? null : new BlockingCollection <TOutput>(Count);

                Name = name;
            }

            public PipelineWorkerAsync(
                    BlockingCollection<TInput>[] input,
                    Action<TInput> renderer,
                    CancellationToken token,
                    string name)
            {
                _input = input;
                _outputProcessor = renderer;
                _token = token;
                Name = name;
                Output = null;
            }

            public BlockingCollection<TOutput>[] Output { get; private set; }

            public string Name { get; private set; }

            public async Task RunAsync()
            {
                Console.WriteLine("{0} is running", this.Name);
                List<Task> tasks = new List<Task>();
                foreach (var bc in _input)
                {
                    var local = bc;
                    var t = Task.Run(new Func<Task>(async () =>
                    {
                        TInput receivedItem;
                        while (!local.IsCompleted && !_token.IsCancellationRequested)
                        {
                            var ok = local.TryTake(out receivedItem, 50, _token);

                            if (ok)
                            {
                                if (Output != null)
                                {
                                    TOutput outputItem = await _processorAsync(receivedItem);
                                    BlockingCollection<TOutput>.AddToAny(Output, outputItem);

                                    Console.WriteLine("{0} sent {1} to next, on thread id {2}",
                                        Name, outputItem, Thread.CurrentThread.ManagedThreadId);
                                    Thread.Sleep(TimeSpan.FromMilliseconds(100));
                                }
                                else
                                {
                                    _outputProcessor(receivedItem);
                                }
                            }
                            else
                            {
                                Thread.Sleep(TimeSpan.FromMilliseconds(50));
                            }
                        }
                    }),
                    _token);

                    tasks.Add(t);
                }

                await Task.WhenAll(tasks);

                if (Output != null)
                {
                    foreach (var bc in Output) bc.CompleteAdding();
                }
            }
        }
    }
}
