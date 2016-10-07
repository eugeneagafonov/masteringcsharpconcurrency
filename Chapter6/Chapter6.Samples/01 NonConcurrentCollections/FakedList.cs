namespace Chapter6.Samples.NonConcurrentCollections
{
    // Fake list of T, used for copy source highlighting to the word
    public class FakedList<T>
    {
        private int _size;
        private T[] _items;
        private int _version;

        // Реализация метода List<T>.Add
        public void Add(T item)
        {
            if (_size == _items.Length) EnsureCapacity(_size + 1);
            _items[_size++] = item;
            _version++;
        }

        private void EnsureCapacity(int i)
        {
            throw new System.NotImplementedException();
        }
    }
}