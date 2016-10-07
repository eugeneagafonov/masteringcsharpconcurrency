using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace Chapter6.Samples._02_ConcurrentCollections.ProducerConsumer.WithCompleteAdding
{
public class BoundedBlockingQueue<T>
{
    private readonly Queue<T> _queue = new Queue<T>(); 
        
    private readonly SemaphoreSlim _nonEmptyQueueSemaphore = 
        new SemaphoreSlim(0, int.MaxValue);

    private readonly CancellationTokenSource _consumersCancellationTokenSource =
        new CancellationTokenSource();

    private readonly SemaphoreSlim _nonFullQueueSemaphore;

    public BoundedBlockingQueue(int boundedCapacity)
    {
        _nonFullQueueSemaphore = new SemaphoreSlim(boundedCapacity);
    }

    public void CompleteAdding()
    {
        // Notify all the consumers that completion is finished
        _consumersCancellationTokenSource.Cancel();
    }

    public void Add(T value)
    {
        _nonFullQueueSemaphore.Wait();

        lock (_queue) _queue.Enqueue(value);
        _nonEmptyQueueSemaphore.Release();
    }

    public T Take()
    {
        T item;
        if (!TryTake(out item))
        {
            throw new InvalidOperationException();
        }

        return item;
    }

    public IEnumerable<T> Consume()
    {
        T element;
        
        while(TryTake(out element))
        {
            yield return element;
        }
    }

    private bool TryTake(out T result)
    {
        result = default(T);

        if (!_nonEmptyQueueSemaphore.Wait(0))
        {
            try
            {
                _nonEmptyQueueSemaphore.Wait(_consumersCancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {
                // Breaking the loop only when cancellation was requested by CompleteAdding
                if (e.CancellationToken == _consumersCancellationTokenSource.Token)
                {
                    return false;
                }

                // Propagate original exception
                throw;
            }
        }
        
        lock (_queue)
        {
            result = _queue.Dequeue();
        }

        _nonFullQueueSemaphore.Release();
        return true;
    }
}
}