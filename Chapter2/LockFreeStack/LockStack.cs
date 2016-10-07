using System.Threading;

namespace ConcurrencyBook.Samples
{
	public class LockStack<T> : StackBase<T> where T : class
	{
		private readonly Mutex _lock = new Mutex();

		public override void Push(T data)
		{
			_lock.WaitOne();
			try
			{
				_head = new Item(data, _head);
			}
			finally
			{
				_lock.ReleaseMutex();
			}
		}

		public override bool TryPop(out T data)
		{
			_lock.WaitOne();
			try
			{
				if (IsEmpty)
				{
					data = null;
					return false;
				}
				data = _head.Data;
				_head = _head.Next;
				return true;
			}
			finally
			{
				_lock.ReleaseMutex();
			}
		}
	}
}