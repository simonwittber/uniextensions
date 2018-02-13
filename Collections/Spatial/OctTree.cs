using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// This class has not been tested.
/// </summary>
namespace DifferentMethods.Extensions.Collections.Spatial
{
    public class OctTree<T>
    {
        int maxObjectsPerCell;
        List<T> objectCollector;
        Bounds bounds;
        OctTree<T>[] cells;
        System.Func<T, Vector3> GetPosition;

        public OctTree(int maxObjectsPerCell, Bounds bounds, System.Func<T, Vector3> GetPosition)
        {
            this.bounds = bounds;
            this.maxObjectsPerCell = maxObjectsPerCell;
            this.GetPosition = GetPosition;
            cells = new OctTree<T>[8];
            objectCollector = new List<T>(maxObjectsPerCell);
        }

        bool IsLeaf
        {
            get { return cells[0] == null; }
        }

        public IEnumerable<List<T>> GetLeafNodes()
        {
            if (IsLeaf)
                yield return objectCollector;
            else
                for (var i = 0; i < 8; i++)
                    foreach (var cell in cells[i].GetLeafNodes())
                        yield return cell;
        }

        public void Insert(T go)
        {
            if (!IsLeaf)
            {
                var cellIndex = GetCellToInsertObject(GetPosition(go));
                if (cellIndex > -1)
                {
                    cells[cellIndex].Insert(go);
                }
                return;
            }
            objectCollector.Add(go);

            if (objectCollector.Count > maxObjectsPerCell)
            {
                if (IsLeaf)
                {
                    float subWidth = (bounds.size.x / 2f);
                    float subHeight = (bounds.size.y / 2f);
                    float subDepth = (bounds.size.z / 2f);
                    float x = bounds.min.x;
                    float y = bounds.min.y;
                    float z = bounds.min.z;
                    cells[0] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x, y, z), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[1] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x, y, z + subDepth), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[2] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x, y + subHeight, z), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[3] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x, y + subHeight, z + subDepth), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[4] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x + subWidth, y, z), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[5] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x + subWidth, y, z + subDepth), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[6] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x + subWidth, y + subHeight, z), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                    cells[7] = new OctTree<T>(maxObjectsPerCell, new Bounds(new Vector3(x + subWidth, y + subHeight, z + subDepth), new Vector3(subWidth, subHeight, subDepth)), GetPosition);
                }

                int i = objectCollector.Count - 1;
                while (i >= 0)
                {
                    var storedObj = objectCollector[i];
                    var cellIndex = GetCellToInsertObject(GetPosition(storedObj));
                    if (cellIndex > -1)
                        cells[cellIndex].Insert(storedObj);
                    else
                        throw new System.Exception("This should never happen.");
                    objectCollector.RemoveAt(i);
                    i--;
                }
            }
        }

        public void Remove(T go)
        {
            if (Contains(GetPosition(go)))
            {
                objectCollector.Remove(go);
                if (cells[0] != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        cells[i].Remove(go);
                    }
                }
            }
        }

        public IEnumerable<T> GetObjects(Bounds area)
        {

            if (area.Intersects(bounds))
            {
                for (int i = 0; i < objectCollector.Count; i++)
                {
                    if (area.Contains(GetPosition(objectCollector[i])))
                    {
                        yield return objectCollector[i];
                    }
                }
                if (cells[0] != null)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        foreach (var o in cells[i].GetObjects(area))
                            yield return o;
                    }
                }
            }
        }


        public void Clear()
        {
            objectCollector.Clear();

            for (var i = 0; i < cells.Length; i++)
            {
                if (cells[i] != null)
                {
                    cells[i].Clear();
                    cells[i] = null;
                }
            }
        }

        public bool Contains(Vector3 location)
        {
            return bounds.Contains(location);
        }

        int GetCellToInsertObject(Vector3 location)
        {
            for (int i = 0; i < 8; i++)
            {
                if (cells[i].Contains(location))
                {
                    return i;
                }
            }
            return -1;
        }

    }
}