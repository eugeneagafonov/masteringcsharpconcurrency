//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using BenchmarkDotNet;
//using BenchmarkDotNet.Tasks;
//using NUnit.Framework;

//namespace Chapter6.Samples.NonConcurrentCollections
//{
//    [TestFixture]
//    public class TestRunner
//    {
//        [Test]
//        public void Run()
//        {
//            var competitionSwitch = new BenchmarkCompetitionSwitch(new[]
//            {
//                typeof (SillyForeachBenchmark)
//            });
            
//            //competitionSwitch.
//            competitionSwitch.Run(new string[]{"1", "1"});
//        }
//    }

//    [Task(processCount: 1)]
//    public class SillyForeachBenchmark
//    {
//        private const int NumberOfElements = 1000000;
//        [Benchmark]
//        public void RunWithLocks()
//        {
//            object syncRoot = new object();
//            var source = Enumerable.Range(1, NumberOfElements).ToList();
//            var destination = new List<int>();

//            Parallel.ForEach(source,
//                n =>
//                {
//                    lock (syncRoot)
//                    {
//                        destination.Add(n);
//                    }
//                });
//        }

//        [Benchmark]
//        public void RunWithOneThread()
//        {
//            object syncRoot = new object();
//            var source = Enumerable.Range(1, NumberOfElements).ToList();
//            var destination = new List<int>();
//            foreach (var n in source)
//            {
//                destination.Add(n);
//            }
//        }
//    }
//}