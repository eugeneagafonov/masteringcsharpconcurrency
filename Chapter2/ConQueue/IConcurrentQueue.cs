namespace ConcurrencyBook.Samples
{
	public interface IConcurrentQueue<T>
	{
		void Enqueue(T data);

		bool TryDequeue(out T data);

		bool IsEmpty { get; }
	}
}