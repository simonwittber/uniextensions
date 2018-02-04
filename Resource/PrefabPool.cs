using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Resource
{

    /// <summary>
    /// Prefab pool. Used for recyling tather than Instantiate/Destroy.
    /// </summary>
    public class PrefabPool
    {

        GameObject prefab;
        List<GameObject> freeObjects = new List<GameObject>();

        /// <summary>
        /// Initializes a new pool of empty objects, and adds the T component to them.
        /// </summary>
        /// <param name="count">Count.</param>
        public PrefabPool(GameObject prefab, int count)
        {
            this.prefab = prefab;
            freeObjects = new List<GameObject>(count);
            for (var i = 0; i < count; i++)
            {
                var g = GameObject.Instantiate(prefab) as GameObject;
                var r = g.AddComponent<Recycler>();
                r.pool = this;
                g.SetActive(false);
                freeObjects.Add(g);
            }
        }

        /// <summary>
        /// Take an object from the pool and set the position and rotation.
        /// </summary>
        /// <param name='position'>
        /// Position.
        /// </param>
        /// <param name='rotation'>
        /// Rotation.
        /// </param>
        public GameObject Take(Vector3 position, Quaternion rotation)
        {
            var g = Take();
            g.transform.position = position;
            g.transform.rotation = rotation;
            return g;
        }

        /// <summary>
        /// Take an object from the pool.
        /// </summary>
        public GameObject Take()
        {
            GameObject g;
            if (freeObjects.Count > 0)
            {
                g = freeObjects.Pop(0);
                g.SetActive(true);
            }
            else
            {
                g = GameObject.Instantiate(prefab) as GameObject;
                var r = g.AddComponent<Recycler>();
                r.pool = this;
            }
            return g;
        }

        /// <summary>
        /// Place an object back into the pool.
        /// </summary>
        /// <param name='g'>
        /// G.
        /// </param>
        public void Recycle(GameObject g)
        {
            g.SetActive(false);
            freeObjects.Add(g);
        }



    }
}



