namespace DifferentMethods.Extensions.Collections
{
    /// <summary>
    /// Lock free Queue, as in Single Writer, Single Reader.
    /// </summary>
    public class LockFreeQueue<T>
    {
        volatile int read, write;
        T[] buffer;

        public LockFreeQueue(int bufferSize)
        {
            buffer = new T[bufferSize];
        }

        public int Count { get; private set; }

        public bool Dequeue(out T item)
        {
            if (read == write)
            {
                item = default(T);
                return false;
            }
            item = buffer[read];
            if ((read + 1) >= buffer.Length)
                read = 0;
            else
                read = read + 1;
            Count--;
            return true;
        }

        public bool Enqueue(T item)
        {
            int newidx;
            if ((write + 1) >= buffer.Length)
                newidx = 0;
            else
                newidx = write + 1;
            if (newidx == read)
                return false;
            buffer[write] = item;
            write = newidx;
            Count++;
            return true;
        }

    }
}

