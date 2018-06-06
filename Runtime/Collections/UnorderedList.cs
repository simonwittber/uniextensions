using System.Collections.Generic;


namespace DifferentMethods.Extensions.Collections
{
    /// <summary>
    /// An unordered list is a regular List, with a O(N) Remove operation. However, order of list is not preserved.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class UnorderedList<T> : List<T>
    {
        public new void Remove(T item)
        {
            this[IndexOf(item)] = this[Count - 1];
            RemoveAt(Count - 1);
        }
    }
}


