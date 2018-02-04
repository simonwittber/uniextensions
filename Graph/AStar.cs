using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DifferentMethods.Extensions.Collections;

namespace DifferentMethods.Extensions.Graph
{
    /// <summary>
    /// The A* path finding algorithm. Uses objects that implement T.
    /// </summary>
    public class AStar<T> where T : class, IAStarNode<T>
    {


        T[] nodes;
        Dictionary<T, float> g;
        Dictionary<T, T> parent;
        Dictionary<T, bool> inPath;
        Queue<T> path;
        HashList<T> openset;
        HashSet<T> closedset;

        public AStar(List<T> nodes)
        {
            this.nodes = nodes.ToArray();
            g = new Dictionary<T, float>();
            parent = new Dictionary<T, T>();
            inPath = new Dictionary<T, bool>();
            openset = new HashList<T>();
            closedset = new HashSet<T>();
            path = new Queue<T>();
        }

        /// <summary>
        /// Calculate the route from start to end.
        /// </summary>
        /// <param name="start">Start.</param>
        /// <param name="end">End.</param>
        public void Route(T start, T end, List<T> route)
        {
            route.Clear();
            if (start == null || end == null)
            {
                return;
            }

            for (int i = 0, nodesLength = nodes.Length; i < nodesLength; i++)
            {
                var s = nodes[i];
                g[s] = 0f;
                parent[s] = null;
                inPath[s] = false;
            }
            openset.Clear();
            closedset.Clear();
            path.Clear();

            var current = start;
            openset.Add(current);
            while (openset.Count > 0)
            {
                current = openset[0];
                for (var i = 1; i < openset.Count; i++)
                {
                    var d = g[current].CompareTo(g[openset[i]]);
                    if (d < 0)
                    {
                        current = openset[i];
                    }
                }
                //openset.Sort ((a,b) => g [a].CompareTo (g [b]));
                current = openset[0];
                if (current == end)
                {
                    while (parent[current] != null)
                    {
                        path.Enqueue(current);
                        inPath[current] = true;
                        current = parent[current];
                        if (path.Count >= nodes.Length)
                        {
                            return;
                        }
                    }
                    inPath[current] = true;
                    path.Enqueue(current);
                    while (path.Count > 0)
                    {
                        route.Add(path.Dequeue());
                    }
                    return;
                }
                openset.Remove(current);
                closedset.Add(current);
                var connectedNodes = current.GetConnectedNodes();
                for (int i = 0, connectedNodesCount = connectedNodes.Count; i < connectedNodesCount; i++)
                {
                    var node = connectedNodes[i];
                    if (closedset.Contains(node))
                        continue;
                    if (openset.Contains(node))
                    {
                        var new_g = g[current] + current.CalculateMoveCost(node);
                        if (g[node] > new_g)
                        {
                            g[node] = new_g;
                            parent[node] = current;
                        }
                    }
                    else
                    {
                        g[node] = g[current] + current.CalculateMoveCost(node);
                        parent[node] = current;
                        openset.Add(node);
                    }
                }
            }
            return;
        }
    }
}
