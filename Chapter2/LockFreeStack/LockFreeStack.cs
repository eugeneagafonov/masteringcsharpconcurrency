using System.Threading;

namespace ConcurrencyBook.Samples
{
	public class LockFreeStack<T> : StackBase<T>
		where T: class 
	{
		public override void Push(T data)
		{
			Item item, oldHead;
			do
			{
				oldHead = _head;
				item = new Item(data, oldHead);
			} while (oldHead != Interlocked.CompareExchange(ref _head, item, oldHead));
		}

		public override bool TryPop(out T data)
		{
			var oldHead = _head;
			while (!IsEmpty)
			{
				if (oldHead == Interlocked.CompareExchange(ref _head, oldHead.Next, oldHead))
				{
					data = oldHead.Data;
					return true;
				}
				oldHead = _head;
			}
			data = null;
			return false;
		}
	}
}