using BenchmarkDotNet;
using BenchmarkDotNet.Tasks;

namespace Chapter6.Benchmarks
{
    [Task(processCount: 1, platform: BenchmarkPlatform.HostPlatform, targetIterationCount: 5, warmupIterationCount: 1,
        mode: BenchmarkMode.SingleRun)]
    public class CompareThreadLocals
    {
        // TODO: finish this test, just to check is my assumption is correct and ConcurrentBag is
        // very slow because thread local is slow in .NET!!
        private const int numberOfThreads = 8;

        [Benchmark]
        public void ThreadLocal()
        {
            
        }
    }
}