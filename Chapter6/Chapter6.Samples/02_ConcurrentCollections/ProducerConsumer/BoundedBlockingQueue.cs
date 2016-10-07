using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Chapter6.Samples._02_ConcurrentCollections.ProducerConsumer
{
    public class BoundedBlockingQueue<T>
    {
        private readonly Queue<T> _queue = new Queue<T>(); 
        
        private readonly SemaphoreSlim _nonEmptyQueueSemaphore = 
            new SemaphoreSlim(0, int.MaxValue);


        private readonly SemaphoreSlim _nonFullQueueSemaphore;

        public BoundedBlockingQueue(int boundedCapacity)
        {
            _nonFullQueueSemaphore = new SemaphoreSlim(boundedCapacity);
        }

        public void Add(T value)
        {
            _nonFullQueueSemaphore.Wait();

            lock (_queue) _queue.Enqueue(value);
            _nonEmptyQueueSemaphore.Release();
        }

        public T Take()
        {
            _nonEmptyQueueSemaphore.Wait();
            T result;
            lock (_queue)
            {
                Debug.Assert(_queue.Count != 0);
                result = _queue.Dequeue();
            }

            _nonFullQueueSemaphore.Release();
            return result;
        }
    }
}