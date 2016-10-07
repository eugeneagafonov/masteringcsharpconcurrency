using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Chapter6.Samples._02_ConcurrentCollections.InvaidImplementation
{
    public class ClassOperationProvider
    {
        private readonly IDictionary<string, OperationResult> _cache =
            new ConcurrentDictionary<string, OperationResult>();

        public OperationResult RunOperationOrGetFromCache(string operationId)
        {
            OperationResult result;
            if (_cache.TryGetValue(operationId, out result))
            {
                return result;
            }
            
            result = RunLongRunningOperation(operationId);
             _cache.Add(operationId, result); // Original version
            return result;
        }

        private OperationResult RunLongRunningOperation(string operationId)
        {
            // Running real long-running operation
            // ...
            Console.WriteLine("Running long-running operation");
            return OperationResult.Create(operationId);
        }
    }


    public class ClassOperationProviderFixed
    {
        private readonly ConcurrentDictionary<string, OperationResult> _cache =
            new ConcurrentDictionary<string, OperationResult>();

        public OperationResult RunOperationOrGetFromCache(string operationId)
        {
            OperationResult result;
            if (_cache.TryGetValue(operationId, out result))
            {
                return result;
            }
            
            result = RunLongRunningOperation(operationId);
            _cache.TryAdd(operationId, result);
            
            return result;
        }

        private OperationResult RunLongRunningOperation(string operationId)
        {
            // Running real long-running operation
            // ...
            Console.WriteLine("Running long-running operation");
            return OperationResult.Create(operationId);
        }
    }
}