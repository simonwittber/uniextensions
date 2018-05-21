using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Collections.Spatial
{

    public class SpatialHash
    {
        Dictionary<int, List<Transform>> idx;
        int cellSize, cellCount;

        public SpatialHash(int cellSize, int area)
        {
            this.cellSize = cellSize;
            this.cellCount = (int)Mathf.Pow(cellSize / cellCount, 3);
            this.idx = new Dictionary<int, List<Transform>>();
        }

        public int Count
        {
            get { return idx.Count; }
        }

        public ICollection<int> Cells
        {
            get { return idx.Keys; }
        }

        public void Insert(Transform tx)
        {
            var v = tx.position;
            List<Transform> cell;
            var keys = Keys(v);
            for (int i = 0, keysCount = keys.Count; i < keysCount; i++)
            {
                var key = keys[i];
                if (idx.ContainsKey(key))
                    cell = idx[key];
                else
                {
                    cell = new List<Transform>();
                    idx.Add(key, cell);
                }
                if (!cell.Contains(tx))
                    cell.Add(tx);
            }
        }

        public List<Transform> Query(Vector3 v)
        {
            var key = Key(v);
            if (idx.ContainsKey(key))
                return idx[key];
            return null;
        }

        List<int> Keys(Vector3 v)
        {
            int o = cellSize / 2;
            var keys = new List<int>();
            keys.Add(Key(new Vector3(v.x - o, v.y - 0, v.z - o)));
            keys.Add(Key(new Vector3(v.x - o, v.y - 0, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - o, v.y - 0, v.z + o)));
            keys.Add(Key(new Vector3(v.x - 0, v.y - 0, v.z - o)));
            keys.Add(Key(new Vector3(v.x - 0, v.y - 0, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - 0, v.y - 0, v.z + o)));
            keys.Add(Key(new Vector3(v.x + o, v.y - 0, v.z - o)));
            keys.Add(Key(new Vector3(v.x + o, v.y - 0, v.z - o)));
            keys.Add(Key(new Vector3(v.x + o, v.y - 0, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - o, v.y - o, v.z - o)));
            keys.Add(Key(new Vector3(v.x - o, v.y - o, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - o, v.y - o, v.z + o)));
            keys.Add(Key(new Vector3(v.x - 0, v.y - o, v.z - o)));
            keys.Add(Key(new Vector3(v.x - 0, v.y - o, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - 0, v.y - o, v.z + o)));
            keys.Add(Key(new Vector3(v.x + o, v.y - o, v.z - o)));
            keys.Add(Key(new Vector3(v.x + o, v.y - o, v.z - o)));
            keys.Add(Key(new Vector3(v.x + o, v.y - o, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - o, v.y + o, v.z - o)));
            keys.Add(Key(new Vector3(v.x - o, v.y + o, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - o, v.y + o, v.z + o)));
            keys.Add(Key(new Vector3(v.x - 0, v.y + o, v.z - o)));
            keys.Add(Key(new Vector3(v.x - 0, v.y + o, v.z - 0)));
            keys.Add(Key(new Vector3(v.x - 0, v.y + o, v.z + o)));
            keys.Add(Key(new Vector3(v.x + o, v.y + o, v.z - o)));
            keys.Add(Key(new Vector3(v.x + o, v.y + o, v.z - o)));
            keys.Add(Key(new Vector3(v.x + o, v.y + o, v.z - 0)));
            return keys;
        }

        int Key(Vector3 v)
        {
            int x = Mathf.FloorToInt(v.x / cellSize) * cellSize;
            int y = Mathf.FloorToInt(v.y / cellSize) * cellSize;
            int z = Mathf.FloorToInt(v.z / cellSize) * cellSize;
            return ((x * 73856093) ^ (y * 19349663) ^ (z * 83492791)) % cellCount;
        }
    }
}