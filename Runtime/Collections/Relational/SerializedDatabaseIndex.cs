using System;
using System.Collections.Generic;
using UnityEngine;

namespace DifferentMethods.Extensions.Collections.Relational
{
    public class SerializedUniqueIndex<T, R> : HashSet<int> where T : SerializedTable<R> where R : ITableRow, new()
    {
        System.Func<R, int> GetID;

        public SerializedUniqueIndex(T table, System.Func<R, int> GetID)
        {
            this.GetID = GetID;
            table.AfterAdd += OnAdd;
            table.AfterUpdate += OnUpdate;
            table.AfterDelete += OnDelete;
            foreach (var i in table)
                OnAdd(i);
        }

        void OnDelete(R row)
        {
            Delete(GetID(row));
        }

        void OnUpdate(R prevRow, R row)
        {
            Update(GetID(prevRow), GetID(row));
        }

        void OnAdd(R row)
        {
            Add(GetID(row));
        }

        public void Update(int oldId, int newId)
        {
            Delete(oldId);
            Add(newId);
        }

        public void Delete(int id)
        {
            Remove(id);
        }
    }

    public class SerializedDatabaseIndex<T, R> : Dictionary<int, HashSet<int>> where T : SerializedTable<R> where R : ITableRow, new()
    {
        System.Func<R, int> GetID;
        System.Func<R, int> GetChildID;

        public SerializedDatabaseIndex(T table, System.Func<R, int> GetChildID)
        {
            Configure(table, GetChildID, q => q.Id);
        }

        public SerializedDatabaseIndex(T table, System.Func<R, int> GetChildID, System.Func<R, int> GetID)
        {
            Configure(table, GetChildID, GetID);
        }

        void Configure(T table, System.Func<R, int> GetChildID, System.Func<R, int> GetID)
        {
            this.GetChildID = GetChildID;
            this.GetID = GetID;
            table.AfterAdd += OnAdd;
            table.AfterUpdate += OnUpdate;
            table.AfterDelete += OnDelete;
            foreach (var i in table)
                OnAdd(i);
        }

        void OnDelete(R row)
        {
            Delete(GetID(row), GetChildID(row));
        }

        void OnUpdate(R prevRow, R row)
        {
            Update(GetID(prevRow), GetChildID(prevRow), GetID(row), GetChildID(row));
        }

        void OnAdd(R row)
        {
            Add(GetID(row), GetChildID(row));
        }

        public bool Contains(int parentId, int childId)
        {
            HashSet<int> children;
            if (TryGetValue(parentId, out children))
                return children.Contains(childId);
            return false;
        }

        public HashSet<int> Get(int parentId)
        {
            HashSet<int> children;
            if (!TryGetValue(parentId, out children))
                children = this[parentId] = new HashSet<int>();
            return children;
        }

        public void Add(int parentId, int childId)
        {
            HashSet<int> children;
            if (!TryGetValue(parentId, out children))
                children = this[parentId] = new HashSet<int>();
            children.Add(childId);
        }

        public void Update(int oldParentId, int oldChildId, int newParentId, int newChildId)
        {
            Delete(oldParentId, oldChildId);
            Add(newParentId, newChildId);
        }

        public void Delete(int parentId, int childId)
        {
            HashSet<int> children;
            if (TryGetValue(parentId, out children))
                children.Remove(childId);
        }
    }
}