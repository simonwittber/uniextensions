using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DifferentMethods.Extensions.Collections.Relational
{

    public class SerializedDatabase : ScriptableObject
    {
        Dictionary<System.Type, ITable> tables;

        public bool IsDirty
        {
            get
            {
                foreach (var i in tables.Values)
                    if (i.IsDirty) return true;
                return false;
            }
        }

        public bool Commit()
        {
            lock (this)
            {
                foreach (var i in tables.Values)
                {
                    if (!i.Verify())
                    {
                        Abort();
                        throw new System.Exception("Not commiting, verification failed on table:" + i.ToString());
                    }
                }
                var dirty = false;
                foreach (var i in tables.Values)
                    if (i.Commit()) dirty = true;
                return dirty;
            }
        }

        public void Abort()
        {
            foreach (var i in tables.Values)
                i.Abort();
        }

        public T GetTable<T>() where T : ITable
        {
            return (T)tables[typeof(T)];
        }

        void OnEnable()
        {
            tables = new Dictionary<System.Type, ITable>();

            foreach (var fi in GetType().GetFields())
            {
                if (fi.FieldType.GetInterfaces().Contains(typeof(ITable)))
                {
                    var table = fi.GetValue(this) as ITable;
                    if (table == null)
                    {
                        table = System.Activator.CreateInstance(fi.FieldType) as ITable;
                        fi.SetValue(this, table);
                    }
                    table.database = this;
                    tables.Add(fi.FieldType, table);
                }
            }
            foreach (var table in tables.Values)
                table.OnEnable();
        }
    }
}