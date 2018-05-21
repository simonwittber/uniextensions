using System;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Collections
{

    /// <summary>
    /// This is a PriorityQueue, which contains only unique items. When an item is added twice, it's priority is changed to the new value.
    /// </summary>
    public class PrioritySet<T> : PriorityQueue<T> where T : IComparable<T>
    {
        HashSet<T> index;

        new public void Clear()
        {
            base.Clear();
            index.Clear();
        }

        new public bool Contains(T item)
        {
            return index.Contains(item);
        }

        public PrioritySet() : base()
        {
            index = new HashSet<T>();
        }

        new public void Push(T item)
        {
            lock (this)
            {
                if (index.Contains(item))
                {
                    base.Remove(item);
                }
                base.Push(item);
                index.Add(item);
            }
        }

        new public T Pop()
        {
            lock (this)
            {
                var item = base.Pop();
                index.Remove(item);
                return item;
            }
        }

    }
}

