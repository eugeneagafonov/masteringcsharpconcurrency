using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet;
using Chapter6.Samples.NonConcurrentCollections;

namespace Chapter6.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            var competitionSwitch = new BenchmarkCompetitionSwitch(new[]
            {
                typeof (SillyForeachBenchmark),
                typeof (ConcurrentBagAndOthers),
                typeof (ConcurrentBagAndOthersWithStrings),
                typeof (ConcurentDictionaryOperations),
            });

            //competitionSwitch.
            competitionSwitch.Run(args);

            Console.WriteLine("Count: " + ConcurrentBagAndOthers.count);
            Console.WriteLine("Press 'Enter' to exit");
            Console.ReadLine();

        }
    }
}
