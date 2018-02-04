using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Collections
{
    public class DefaultDict<K, V> : IDictionary<K, V>
    {
        Dictionary<K, V> dictionary;
        System.Func<K, V> defaultValue;
        public DefaultDict(System.Func<K, V> defaultValue)
        {
            this.defaultValue = defaultValue;
        }

        public V this[K key]
        {
            get
            {
                V value;
                if (dictionary.TryGetValue(key, out value))
                    return value;
                value = dictionary[key] = defaultValue(key);
                return value;
            }
            set
            {
                ((IDictionary<K, V>)dictionary)[key] = value;
            }
        }

        public ICollection<K> Keys => ((IDictionary<K, V>)dictionary).Keys;

        public ICollection<V> Values => ((IDictionary<K, V>)dictionary).Values;

        public int Count => ((IDictionary<K, V>)dictionary).Count;

        public bool IsReadOnly => ((IDictionary<K, V>)dictionary).IsReadOnly;

        public void Add(K key, V value)
        {
            ((IDictionary<K, V>)dictionary).Add(key, value);
        }

        public void Add(KeyValuePair<K, V> item)
        {
            ((IDictionary<K, V>)dictionary).Add(item);
        }

        public void Clear()
        {
            ((IDictionary<K, V>)dictionary).Clear();
        }

        public bool Contains(KeyValuePair<K, V> item)
        {
            return ((IDictionary<K, V>)dictionary).Contains(item);
        }

        public bool ContainsKey(K key)
        {
            return ((IDictionary<K, V>)dictionary).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            ((IDictionary<K, V>)dictionary).CopyTo(array, arrayIndex);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return ((IDictionary<K, V>)dictionary).GetEnumerator();
        }

        public bool Remove(K key)
        {
            return ((IDictionary<K, V>)dictionary).Remove(key);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return ((IDictionary<K, V>)dictionary).Remove(item);
        }

        public bool TryGetValue(K key, out V value)
        {
            return ((IDictionary<K, V>)dictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<K, V>)dictionary).GetEnumerator();
        }
    }
}
