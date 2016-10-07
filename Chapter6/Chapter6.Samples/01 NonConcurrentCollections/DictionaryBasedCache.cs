using System.Collections.Generic;
using System.Threading;

namespace Chapter6.Samples.NonConcurrentCollections
{
    public class OperationResult
    {
        
    }

    public class ClassOperationProvider
    {
        private readonly Dictionary<string, OperationResult> _cache = new Dictionary<string, OperationResult>();
        private readonly ReaderWriterLockSlim _rwLockSlim = new ReaderWriterLockSlim();

        public OperationResult RunOperationOrGetFromCache(string operationId)
        {
            _rwLockSlim.EnterReadLock();
            try
            {
                OperationResult result;
                if (_cache.TryGetValue(operationId, out result))
                    return result;
            }
            finally
            {
                _rwLockSlim.ExitReadLock();
            }

            // Поток 1
            _rwLockSlim.EnterWriteLock();

            try
            {
                OperationResult result;
                if (_cache.TryGetValue(operationId, out result))
                    return result;

                result = RunLongRunningOperation(operationId);
                _cache.Add(operationId, result);
                return result;
            }
            finally
            {
                _rwLockSlim.ExitReadLock();
            }
        }

        private OperationResult RunLongRunningOperation(string operationId)
        {
            // Running real long-running operation
            // ...
            throw new System.NotImplementedException();
        }
    }
}