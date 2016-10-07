using System.Collections.Concurrent;

namespace ConcurrencyBook.Samples
{
	public class ConcurrentStackWrapper<T> : StackBase<T>
	{
		private readonly ConcurrentStack<T> _stack; 
		public ConcurrentStackWrapper()
		{
			_stack = new ConcurrentStack<T>();
		}

		public override void Push(T data)
		{
			_stack.Push(data);
		}

		public override bool TryPop(out T data)
		{
			return _stack.TryPop(out data);
		}
	}
}