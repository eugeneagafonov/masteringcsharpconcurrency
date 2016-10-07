namespace ConcurrencyBook.Samples
{
	public class MonitorStack<T> : StackBase<T> where T: class
	{
		private readonly object _lock = new object();

		public override void Push(T data)
		{
			lock (_lock)
				_head = new Item(data, _head);
		}

		public override bool TryPop(out T data)
		{
			lock (_lock)
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
		}
	}
}