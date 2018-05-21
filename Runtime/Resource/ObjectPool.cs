using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Resource
{

    /// <summary>
    /// Object pool. Used for recyling tather than Instantiate/Destroy.
    /// </summary>
    public class ObjectPool<T> where T : new()
    {
        List<T> freeObjects;

        /// <summary>
        /// Initializes a new pool of empty objects.
        /// </summary>
        /// <param name="count">Count.</param>
        public ObjectPool(int count)
        {
            freeObjects = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                var g = new T();
                freeObjects.Add(g);
            }
        }

        /// <summary>
        /// Take an object from the pool.
        /// </summary>
        public T Take()
        {
            T g;
            if (freeObjects.Count > 0)
            {
                g = freeObjects.Pop(0);
            }
            else
            {
                g = new T();
            }
            return g;
        }

        /// <summary>
        /// Place an object back into the pool.
        /// </summary>
        /// <param name='g'>
        /// G.
        /// </param>
        public void Recycle(T g)
        {
            freeObjects.Add(g);
        }

    }
}



