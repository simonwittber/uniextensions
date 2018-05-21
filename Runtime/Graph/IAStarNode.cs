using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Interface required for objects to be used with the AStar path finding class.
/// </summary>
namespace DifferentMethods.Extensions.Graph
{
    public interface IAStarNode<T> where T : class
    {
        /// <summary>
        /// Gets all nodes that are connected to this node.
        /// </summary>
        /// <returns>The connected nodes.</returns>
        List<T> GetConnectedNodes();

        /// <summary>
        /// Calculates the estimated move cost to another node.
        /// </summary>
        /// <returns>The move cost.</returns>
        /// <param name="node">Node.</param>
        float CalculateMoveCost(T node);

    }
}