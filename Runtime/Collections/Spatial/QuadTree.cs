using UnityEngine;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Collections.Spatial
{
    public class QuadTree<T>
    {
        int maxObjectsPerCell;
        List<T> objectCollector;
        Rect bounds;
        QuadTree<T>[] cells;
        System.Func<T, Vector2> GetPosition;

        public QuadTree(int maxObjectsPerCell, Rect bounds, System.Func<T, Vector2> GetPosition)
        {
            this.bounds = bounds;
            this.maxObjectsPerCell = maxObjectsPerCell;
            this.GetPosition = GetPosition;
            cells = new QuadTree<T>[4];
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
                for (var i = 0; i < 4; i++)
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
            //Objects exceed the maximum count
            if (objectCollector.Count > maxObjectsPerCell)
            {
                //Split the quad into 4 sections
                if (IsLeaf)
                {
                    float subWidth = (bounds.width / 2f);
                    float subHeight = (bounds.height / 2f);
                    float x = bounds.x;
                    float y = bounds.y;
                    cells[0] = new QuadTree<T>(maxObjectsPerCell, new Rect(x + subWidth, y, subWidth, subHeight), GetPosition);
                    cells[1] = new QuadTree<T>(maxObjectsPerCell, new Rect(x, y, subWidth, subHeight), GetPosition);
                    cells[2] = new QuadTree<T>(maxObjectsPerCell, new Rect(x, y + subHeight, subWidth, subHeight), GetPosition);
                    cells[3] = new QuadTree<T>(maxObjectsPerCell, new Rect(x + subWidth, y + subHeight, subWidth, subHeight), GetPosition);
                }
                //Reallocate this quads objects into its children
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
                    for (int i = 0; i < 4; i++)
                    {
                        cells[i].Remove(go);
                    }
                }
            }
        }

        public IEnumerable<T> GetObjects(Rect area)
        {

            if (RectOverlap(bounds, area))
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
                    for (int i = 0; i < 4; i++)
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

        public bool Contains(Vector2 location)
        {
            return bounds.Contains(location);
        }

        int GetCellToInsertObject(Vector2 location)
        {
            for (int i = 0; i < 4; i++)
            {
                if (cells[i].Contains(location))
                {
                    return i;
                }
            }
            return -1;
        }

        bool ValueInRange(float value, float min, float max)
        {
            return (value >= min) && (value <= max);
        }

        bool RectOverlap(Rect A, Rect B)
        {
            return
            (ValueInRange(A.x, B.x, B.x + B.width) || ValueInRange(B.x, A.x, A.x + A.width))
            &&
            (ValueInRange(A.y, B.y, B.y + B.height) || ValueInRange(B.y, A.y, A.y + A.height));
        }

        public void DrawDebug()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(bounds.x, 0, bounds.y), new Vector3(bounds.x, 0, bounds.y + bounds.height));
            Gizmos.DrawLine(new Vector3(bounds.x, 0, bounds.y), new Vector3(bounds.x + bounds.width, 0, bounds.y));
            Gizmos.DrawLine(new Vector3(bounds.x + bounds.width, 0, bounds.y), new Vector3(bounds.x + bounds.width, 0, bounds.y + bounds.height));
            Gizmos.DrawLine(new Vector3(bounds.x, 0, bounds.y + bounds.height), new Vector3(bounds.x + bounds.width, 0, bounds.y + bounds.height));
            if (cells[0] != null)
            {
                for (int i = 0; i < cells.Length; i++)
                {
                    if (cells[i] != null)
                    {
                        cells[i].DrawDebug();
                    }
                }
            }
        }
    }
}