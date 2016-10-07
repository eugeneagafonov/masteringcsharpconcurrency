using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Chapter6.Samples._02_ConcurrentCollections
{
    public class OperationResult
    {
        public static OperationResult Create(string operationId)
        {
            return new OperationResult();
        }
    }

public class CustomProvider
{
    //private readonly ConcurrentDictionary<string, Lazy<OperationResult>> _cache = 
    //    new ConcurrentDictionary<string, Lazy<OperationResult>>();

    private readonly ConcurrentDictionary<string, OperationResult> _cache =
        new ConcurrentDictionary<string, OperationResult>();

    private int _runLongRunningOperationsNumberOfCalls;

    public int LongRunningOperationNumberOfCalls
    {
        get { return Volatile.Read(ref _runLongRunningOperationsNumberOfCalls); }
    }

    public OperationResult RunOperationOrGetFromCache(string operationId)
    {
        return _cache.GetOrAdd(operationId, id => RunLongRunningOperation(id));
        //return _cache.GetOrAdd(operationId, id => RunLongRunningOperation(id));
    }

    private OperationResult RunLongRunningOperation(string operationId)
    {
        Interlocked.Increment(ref _runLongRunningOperationsNumberOfCalls);

        // Running real long-running operation
        // ...
        Thread.Sleep(10);
        Console.WriteLine("Running long-running operation");
        return OperationResult.Create(operationId);
    }
}
    
public class CustomProviderWithLazyTrick
{
    private readonly ConcurrentDictionary<string, Lazy<OperationResult>> _cache =
        new ConcurrentDictionary<string, Lazy<OperationResult>>();

    public OperationResult RunOperationOrGetFromCache(string operationId)
    {
        return _cache.GetOrAdd(operationId, 
            id => new Lazy<OperationResult>(() => RunLongRunningOperation(id))).Value;
    }

    private OperationResult RunLongRunningOperation(string operationId)
    {
        Interlocked.Increment(ref _runLongRunningOperationsNumberOfCalls);

        // Running real long-running operation
        // ...
        Thread.Sleep(10);
        Console.WriteLine("Running long-running operation");
        return OperationResult.Create(operationId);
    }

    private int _runLongRunningOperationsNumberOfCalls;

    public int LongRunningOperationNumberOfCalls
    {
        get { return Volatile.Read(ref _runLongRunningOperationsNumberOfCalls); }
    }
}

    [TestFixture]
    public class ClassOperationProviderTest
    {
        [Test]
        public async Task RunFromMultipleThreads()
        {
            var provider = new CustomProvider();
            const int numberOfThreads = 10;
            var tasks = 
                Enumerable.Range(1, numberOfThreads)
                .Select(_ => Task.Factory.StartNew(() => provider.RunOperationOrGetFromCache("42")))
                .ToArray();

            var results = await Task.WhenAll(tasks);
            var hs = new HashSet<OperationResult>(results);
            Console.WriteLine("Hashset count: " + hs.Count);
            //CollectionAssert.AllItemsAreUnique();
            Console.WriteLine(provider.LongRunningOperationNumberOfCalls);
        }
    }

}