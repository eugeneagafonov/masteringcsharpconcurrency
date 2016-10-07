using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace Chapter6.Samples._02_ConcurrentCollections.ProducerConsumer
{
    public class CustomProducerConsumer<T> : IDisposable
    {
        private readonly Action<T> _consumeItem;
        private readonly BlockingCollection<T> _blockingCollection;
        private readonly Task[] _workers;

        public CustomProducerConsumer(Action<T> consumeItem, int degreeOfParallelism, int capacity = 1024)
        {
            _consumeItem = consumeItem;
            
            _blockingCollection = new BlockingCollection<T>(capacity);
            
            _workers = Enumerable.Range(1, degreeOfParallelism)
                .Select(_ => Task.Factory.StartNew(Worker, TaskCreationOptions.LongRunning))
                .ToArray();
        }

        public void Process(T item)
        {
            _blockingCollection.Add(item);
        }

        public void CompleteProcessing()
        {
            _blockingCollection.CompleteAdding();
        }

        public void Dispose()
        {
            // Unblock all workers even if the client
            // didn't call CompleteProcessing
            if (!_blockingCollection.IsAddingCompleted)
            {
                _blockingCollection.CompleteAdding();
            }

            Task.WaitAll(_workers);

            _blockingCollection.Dispose();
        }

        private void Worker()
        {
            foreach (var item in _blockingCollection.GetConsumingEnumerable())
            {
                _consumeItem(item);
            }
        }
    }
}