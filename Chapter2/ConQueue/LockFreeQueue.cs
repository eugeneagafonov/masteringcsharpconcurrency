using System.Threading;

namespace ConcurrencyBook.Samples
{
	public class LockFreeQueue<T> 
	{
		protected class Item
		{
			public T Data;

			public Item Next;
		}

		private Item _head;
		private Item _tail;

		public LockFreeQueue()
		{
			_head = new Item();
			_tail = _head;
		}

		public void Enqueue(T data)
		{
			var item = new Item {Data = data};

			Item oldTail = null;

			// We repeat enqueueing operation until it succeeds
			var update = false;
			while (!update)
			{
				// grab copies
				oldTail = _tail;
				var oldNext = oldTail.Next;

				Thread.MemoryBarrier();

				// was _tail already updated ?
				if (_tail == oldTail)
					// and is _tail still the last item in the queue?
					if (oldNext == null)
					{
						// then we're trying to update actual tail's next reference to be enqueueing item
						update = Interlocked.CompareExchange(ref _tail.Next, item, null) == null;
					}
					else
					{
						// it means that another thread is enqueueing a new item right now, so we should try
						// to set the tail reference to point to its next node
						Interlocked.CompareExchange(ref _tail, oldNext, oldTail);
					}
			}
			// Here we have successfully inserted new item to the end of the queue, and now we’re trying 
			// to update tail reference. But if we fail it is ok since another thread will eventually 
			// do this in its Enqueue method call
			Interlocked.CompareExchange(ref _tail, item, oldTail);
		}

		public bool TryDequeue(out T result)
		{
			result = default(T);
			Item oldNext = null;

			// Create a loop that finishes either if there is nothing to dequeue or we dequeued
			// an item succesfully
			var advanced = false;
			while (!advanced)
			{
				// grab copies
				var oldHead = _head;
				var oldTail = _tail;
				oldNext = oldHead.Next;

				// full fence to prevent reordering
				Thread.MemoryBarrier();

				// if head have not been changed yet
				if (oldHead == _head)
				{
					// is the queue empty?
					if (oldHead == oldTail)
					{
						// if so, this should be false then
						if (oldNext != null)
						{
							// if not then we have lagging tail, we need to update it
							Interlocked.CompareExchange(ref _tail, oldNext, oldTail);
							continue;
						}

						// empty queue - return null and false
						result = default(T);
						return false;
					}

					// Now get the dequeueing item, and try to advance the head reference
					// success means success of the whole operation
					result = oldNext.Data;
					advanced = Interlocked.CompareExchange(ref _head, oldNext, oldHead) == oldHead;
				}
			}

			// remove any references that can prevent garbage collector to do its job
			oldNext.Data = default(T);
			return true;
		}

		public bool IsEmpty
		{
			get
			{
				return _head == _tail;
			}
		}

	}
}