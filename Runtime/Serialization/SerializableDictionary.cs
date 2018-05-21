using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// A Fixed list is simlar to a regular generic list, except that it is of a fixed size, and you can
/// access the internal array of values via the .Buffer property.
/// </summary>

namespace DifferentMethods.Extensions.Serialization
{
    public abstract class SerializableDictionary<K, V> : ISerializationCallbackReceiver, IDictionary<K, V>
    {
        [SerializeField]
        private K[] keys;
        [SerializeField]
        private V[] values;

        Dictionary<K, V> dictionary = new Dictionary<K, V>();

        public ICollection<K> Keys => ((IDictionary<K, V>)dictionary).Keys;

        public ICollection<V> Values => ((IDictionary<K, V>)dictionary).Values;

        public int Count => ((IDictionary<K, V>)dictionary).Count;

        public bool IsReadOnly => ((IDictionary<K, V>)dictionary).IsReadOnly;

        public V this[K key]
        {
            get
            {
                return dictionary[key];
            }
            set
            {
                dictionary[key] = value;
            }
        }

        public void Set(K k, object v)
        {
            this[k] = (V)v;
        }

        public V Get(K k)
        {
            V v;
            if (dictionary.TryGetValue(k, out v))
                return v;
            return default(V);
        }

        public void OnAfterDeserialize()
        {
            if (keys == null || values == null) return;
            var c = keys.Length;
            dictionary = new Dictionary<K, V>(c);
            for (int i = 0; i < c; i++)
            {
                dictionary[keys[i]] = values[i];
            }
            keys = null;
            values = null;
        }

        public void OnBeforeSerialize()
        {
            var c = dictionary.Count;
            keys = new K[c];
            values = new V[c];
            int i = 0;
            using (var e = dictionary.GetEnumerator())
                while (e.MoveNext())
                {
                    var kvp = e.Current;
                    keys[i] = kvp.Key;
                    values[i] = kvp.Value;
                    i++;
                }
        }

        public void Add(K key, V value)
        {
            ((IDictionary<K, V>)dictionary).Add(key, value);
        }

        public bool ContainsKey(K key)
        {
            return ((IDictionary<K, V>)dictionary).ContainsKey(key);
        }

        public bool Remove(K key)
        {
            return ((IDictionary<K, V>)dictionary).Remove(key);
        }

        public bool TryGetValue(K key, out V value)
        {
            return ((IDictionary<K, V>)dictionary).TryGetValue(key, out value);
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

        public void CopyTo(KeyValuePair<K, V>[] array, int arrayIndex)
        {
            ((IDictionary<K, V>)dictionary).CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<K, V> item)
        {
            return ((IDictionary<K, V>)dictionary).Remove(item);
        }

        public IEnumerator<KeyValuePair<K, V>> GetEnumerator()
        {
            return ((IDictionary<K, V>)dictionary).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<K, V>)dictionary).GetEnumerator();
        }
    }



}