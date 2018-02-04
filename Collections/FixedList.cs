using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

/// <summary>
/// A Fixed list is simlar to a regular generic list, except that it is of a fixed size, and you can
/// access the internal array of values via the .Buffer property.
/// </summary>

namespace DifferentMethods.Extensions.Collections
{

    public class FixedList<T> : System.Collections.Generic.IList<T> where T : System.IComparable
    {
        public T[] Buffer
        {
            get;
            private set;
        }

        public FixedList(int size)
        {
            this.Buffer = new T[size];
        }

        public FixedList(T[] array)
        {
            this.Buffer = array;
        }

        public void AddRange(IEnumerable<T> range)
        {
            foreach (var item in range)
            {
                Buffer[Count++] = item;
            }
        }

        public static implicit operator T[] (FixedList<T> fl)
        {
            return fl.Buffer;
        }

        public static implicit operator FixedList<T>(T[] a)
        {
            return new FixedList<T>(a);
        }

        #region IList implementation

        public int IndexOf(T item)
        {
            return Array.IndexOf(Buffer, item, 0, Count);
        }

        public void Insert(int index, T item)
        {
            for (var i = Count; i > index; i--)
            {
                Buffer[i] = Buffer[i - 1];
            }
            Buffer[index] = item;
            Count++;
        }

        public void RemoveAt(int index)
        {
            if (index >= Count)
                throw new System.IndexOutOfRangeException();
            for (var i = index; i < Count; i++)
            {
                Buffer[i] = Buffer[i + 1];
            }
        }

        public T this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new System.IndexOutOfRangeException();
                return Buffer[index];
            }
            set
            {
                if (index >= Count)
                    throw new System.IndexOutOfRangeException();
                Buffer[index] = value;
            }
        }

        #endregion

        #region ICollection implementation

        public void Add(T item)
        {
            Buffer[Count++] = item;
        }

        public void Clear()
        {
            Count = 0;
        }

        public bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            System.Array.Copy(Buffer, 0, array, arrayIndex, Count);
        }

        public bool Remove(T item)
        {
            var index = IndexOf(item);
            if (index >= 0)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public int Count
        {
            get;
            private set;
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        #endregion

        #region IEnumerable implementation

        public System.Collections.Generic.IEnumerator<T> GetEnumerator()
        {
            foreach (var i in Buffer)
                yield return i;
        }

        #endregion

        #region IEnumerable implementation

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new System.NotImplementedException();
        }

        #endregion

        public override string ToString()
        {
            return "[" + string.Join(",", (from i in Buffer.Take(16)
                                           select i.ToString()).ToArray()) + (Buffer.Length > 16 ? "..." : "") + "]";
        }
    }



}