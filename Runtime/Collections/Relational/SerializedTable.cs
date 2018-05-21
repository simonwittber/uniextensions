using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DifferentMethods.Extensions.Collections.Relational
{

    [System.Serializable]
    public class SerializedTable<T> : ITable, ISerializationCallbackReceiver, ICollection<T> where T : ITableRow, new()
    {
        [SerializeField] T[] items;
        [NonSerialized] List<T> updates = new List<T>();
        [NonSerialized] List<T> deletions = new List<T>();
        [NonSerialized] List<T> additions = new List<T>();
        [NonSerialized] int guid = 1;
        [NonSerialized] Dictionary<int, T> rows = new Dictionary<int, T>();

        public event Action<T, T> AfterUpdate;
        public event Action<T> AfterAdd;
        public event Action<T> AfterDelete;

        public SerializedDatabase database { get; set; }

        public int Count => rows.Count;

        public bool IsReadOnly => false;

        public virtual void OnEnable()
        {
            Apply((A) =>
            {
                A.DB = database;
                return A;
            });
        }

        public virtual T New()
        {
            lock (this)
            {
                var row = new T() { Id = guid++ };
                row.DB = database;
                return row;
            }
        }

        public virtual void OnUpdate(T row, ref T updated)
        {
        }

        public virtual void OnAdd(ref T row)
        {
        }

        public virtual void OnDelete(T row)
        {
        }

        public virtual bool CheckOnUpdate(T current, T row)
        {
            return true;
        }

        public virtual bool CheckOnCreate(T row)
        {
            return true;
        }

        public virtual bool CheckOnDelete(T row)
        {
            return true;
        }

        public bool Verify()
        {
            foreach (var item in updates)
            {
                var current = rows[item.Id];
                if (!CheckOnUpdate(current, item))
                {
                    Debug.Log("CheckOnUpdate failed.");
                    return false;
                };
                if (!(current.RowVersion == item.RowVersion))
                {
                    Debug.Log("Row versioning failed.");
                    return false;
                };
            }
            foreach (var item in additions)
                if (!CheckOnCreate(item))
                {
                    Debug.Log("CheckOnCreate failed.");
                    return false;
                };
            foreach (var item in deletions)
                if (!CheckOnDelete(item))
                {
                    Debug.Log("CheckDelete failed.");
                    return false;
                };
            return true;
        }

        public void Update(T item)
        {
            OnUpdate(rows[item.Id], ref item);
            updates.Add(item);
        }

        public bool UpdateAndCommit(T item)
        {
            OnUpdate(rows[item.Id], ref item);
            updates.Add(item);
            return Commit();
        }

        public void Apply(System.Func<T, T> fn, System.Func<T, bool> q)
        {
            foreach (var i in Where(q))
            {
                var ni = fn(i);
                OnUpdate(rows[i.Id], ref ni);
                updates.Add(ni);
            }
        }

        public void Apply(System.Func<T, T> fn)
        {
            foreach (var i in this)
            {
                var ni = fn(i);
                OnUpdate(rows[i.Id], ref ni);
                updates.Add(ni);
            }
        }

        public void Abort()
        {
            updates.Clear();
            additions.Clear();
            deletions.Clear();
        }

        public bool IsDirty
        {
            get
            {
                return additions.Count > 0 || updates.Count > 0 || deletions.Count > 0;
            }
        }

        public ICollection<int> Keys => ((IDictionary<int, T>)rows).Keys;

        public ICollection<T> Values => ((IDictionary<int, T>)rows).Values;

        int[] ITable.Keys => Keys.ToArray();

        public T this[int key]
        {
            get
            {
                return ((IDictionary<int, T>)rows)[key];
            }
            set
            {
                if (rows.ContainsKey(key))
                    updates.Add(value);
                else
                    additions.Add(value);
            }
        }

        ITableRow ITable.this[int key]
        {
            get
            {
                return this.rows[key];
            }
        }

        public IEnumerable<T> Where(System.Func<T, bool> q)
        {
            foreach (var i in rows.Values)
                if (q(i)) yield return i;
        }

        public bool Exists(System.Func<T, bool> q)
        {
            foreach (var i in rows.Values)
                if (q(i)) return true;
            return false;
        }

        public bool Commit()
        {
            var dirty = false;
            lock (this)
            {
                try
                {
                    foreach (var item in updates)
                    {
                        if (item.Id == 0) continue;
                        dirty = true;
                        item.RowVersion++;
                        var oldItem = rows[item.Id];
                        rows[item.Id] = item;
                        try
                        {
                            if (AfterUpdate != null) AfterUpdate(oldItem, item);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                    foreach (var item in additions)
                    {
                        if (item.Id == 0) continue;
                        dirty = true;
                        guid = item.Id > guid ? item.Id + 1 : guid;
                        rows.Add(item.Id, item);
                        try
                        {
                            if (AfterAdd != null) AfterAdd(item);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                    foreach (var item in deletions)
                    {
                        if (item.Id == 0) continue;
                        dirty = true;
                        rows.Remove(item.Id);
                        try
                        {
                            if (AfterDelete != null) AfterDelete(item);
                        }
                        catch (Exception e)
                        {
                            Debug.LogException(e);
                        }
                    }
                }
                finally
                {
                    updates.Clear();
                    additions.Clear();
                    deletions.Clear();
                }
            }
            return dirty;
        }


        public void Add(T item)
        {
            OnAdd(ref item);
            additions.Add(item);
        }

        public bool AddAndCommit(T item)
        {
            OnAdd(ref item);
            additions.Add(item);
            return Commit();
        }

        public void Clear()
        {
            rows.Clear();

        }

        public HashSet<int> KeySet()
        {
            return new HashSet<int>(rows.Keys);
        }

        public bool ContainsKey(int id)
        {
            if (id == 0) return false;
            return rows.ContainsKey(id);
        }

        public bool Contains(T item)
        {
            return rows.ContainsKey(item.Id);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            rows.Values.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return rows.Values.GetEnumerator();
        }

        public void OnAfterDeserialize()
        {
            Clear();
            if (items != null)
            {
                foreach (var i in items)
                    Add(i);
                Commit();
            }
            guid++;
        }

        public void OnBeforeSerialize()
        {
            items = new T[rows.Count];
            CopyTo(items, 0);
        }

        public void Remove(System.Func<T, bool> where)
        {
            foreach (var i in this)
                if (where(i)) Remove(i);
        }

        public void Remove(int id)
        {
            Remove(this[id]);
        }

        public bool Remove(T item)
        {
            if (rows.ContainsKey(item.Id))
            {
                deletions.Add(item);
                OnDelete(item);
                return true;
            }
            return false;
        }

        public bool TryGetValue(int id, out T i)
        {
            return rows.TryGetValue(id, out i);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return rows.Values.GetEnumerator();
        }


    }
}