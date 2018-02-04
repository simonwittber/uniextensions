using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Collections
{

    /// <summary>
    /// A Ring Buffer.
    /// </summary>
    /// <exception cref='IndexOutOfRangeException'>
    /// Is thrown when an attempt is made to access an element of an array with an index that is outside the bounds of the array.
    /// </exception>
    public class RingBuffer<T>
    {
        private volatile int size;
        private volatile int read = 0;
        private volatile int write = 0;
        private volatile int count = 0;
        private volatile T[] objects;



        public int AvailableToRead
        {
            get
            {
                return count;
            }
        }

        public int AvailableToWrite
        {
            get
            {
                return size - count;
            }
        }

        public void Clear()
        {
            read = write = count = 0;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RingBuffer`1"/> class with a maximum capacity.
        /// </summary>
        /// <param name='size'>
        /// Size.
        /// </param>
        public RingBuffer(int size)
        {
            this.size = size;
            objects = new T[size + 1];
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RingBuffer`1"/> is empty.
        /// </summary>
        /// <value>
        /// <c>true</c> if empty; otherwise, <c>false</c>.
        /// </value>
        public bool Empty
        {
            get { return (read == write) && (count == 0); }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="RingBuffer`1"/> is full.
        /// </summary>
        /// <value>
        /// <c>true</c> if full; otherwise, <c>false</c>.
        /// </value>
        public bool Full
        {
            get { return (read == write) && (count > 0); }
        }

        public void Write(T[] src)
        {
            Write(src, src.Length);
        }
        /// <summary>
        /// Write the specified src array into the buffer.
        /// </summary>
        /// <param name='src'>
        /// Source.
        /// </param>
        /// <exception cref='IndexOutOfRangeException'>
        /// Is thrown when no room is left for the array.
        /// </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Write(T[] src, int srcLength)
        {
            if (Full)
                throw new IndexOutOfRangeException("Queue Full!");
            if (size - count < srcLength)
                throw new IndexOutOfRangeException("Queue Almost Full!");

            if (write > read && size - write < srcLength)
            {
                var b = size - write;
                CopyArray(src, 0, objects, write, b);
                count += b;
                write = (write + b) % size;
                var r = srcLength - b;
                CopyArray(src, b, objects, write, r);
                count += r;
                write = (write + r) % size;
            }
            else
            {
                CopyArray(src, 0, objects, write, srcLength);
                count += srcLength;
                write = (write + srcLength) % size;
            }
        }

        /// <summary>
        /// Write the specified items into the buffer.
        /// </summary>
        /// <param name="items">Items.</param>
        public void Write(IEnumerable<T> items)
        {
            foreach (var i in items)
            {
                Write(i);
            }
        }

        /// <summary>
        /// Write a single item into the buffer.
        /// </summary>
        /// <param name='item'>
        /// Item.
        /// </param>
        /// <exception cref='IndexOutOfRangeException'>
        /// Is thrown when there is no room in the buffer.
        /// </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Write(T item)
        {

            if (Full)
                throw new IndexOutOfRangeException("Queue Full!");

            objects[write] = item;
            count++;
            write = (write + 1) % size;
        }

        /// <summary>
        /// Read from the buffer into the dest array.
        /// </summary>
        /// <param name='dest'>
        /// Destination.
        /// </param>
        /// <exception cref='IndexOutOfRangeException'>
        /// Is thrown when an attempt is made to read more data than is available
        /// </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void Read(T[] dest)
        {
            if (Empty)
                throw new IndexOutOfRangeException("Queue Empty!");
            if (count < dest.Length)
                throw new IndexOutOfRangeException("Queue Almost Empty!");

            if (size - read >= dest.Length)
            {
                CopyArray(objects, read, dest, 0, dest.Length);
                count -= dest.Length;
                read = (read + dest.Length) % size;
            }
            else
            {
                var b = size - read;
                CopyArray(objects, read, dest, 0, b);
                count -= b;
                read = (read + b) % size;
                var r = dest.Length - b;
                CopyArray(objects, read, dest, b, r);
                count -= r;
                read = (read + r) % size;
            }
        }

        public void Discard(int count)
        {
            this.count -= count;
            read = (read + count) % size;
        }

        /// <summary>
        /// Read a single item from the buffer.
        /// </summary>
        /// <exception cref='IndexOutOfRangeException'>
        /// Is thrown when an attempt is made to read more data than is available
        /// </exception>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public T Read()
        {
            if (Empty)
                throw new IndexOutOfRangeException("Queue Empty!");

            T item = objects[read];
            count--;
            read = (read + 1) % size;
            return item;
        }

        List<System.ArraySegment<T>> segments = new List<System.ArraySegment<T>>(2);

        public List<ArraySegment<T>> GetSegments()
        {
            segments.Clear();
            if (size - read >= count)
            {
                segments.Add(new ArraySegment<T>(objects, read, count));
            }
            else
            {
                segments.Add(new ArraySegment<T>(objects, read, size - read));
                segments.Add(new ArraySegment<T>(objects, (read + (size - read)) % size, count - (size - read)));
            }
            return segments;
        }

        public void CopyInto(RingBuffer<T> other)
        {
            CopyArray(this.objects, 0, other.objects, 0, size);
            other.read = read;
            other.count = count;
            other.write = write;
        }

        void CopyArray(T[] src, int srcIndex, T[] dst, int dstIndex, int count)
        {
            for (int i = 0; i < count; i++)
            {
                dst[i + dstIndex] = src[i + srcIndex];
            }
        }
    }
}
