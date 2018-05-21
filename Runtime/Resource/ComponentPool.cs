using System;

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions.Resource
{

    /// <summary>
    /// Object pool. Used for recyling tather than Instantiate/Destroy.
    /// </summary>
    public class ComponentPool<T> where T : MonoBehaviour
    {
        List<T> freeObjects;

        /// <summary>
        /// Initializes a new pool of empty objects, and adds the T component to them.
        /// </summary>
        /// <param name="count">Count.</param>
        public ComponentPool(int count)
        {
            freeObjects = new List<T>(count);
            for (var i = 0; i < count; i++)
            {
                var g = new GameObject("Pooled Object", typeof(T)).GetComponent<T>();
                g.gameObject.SetActive(false);
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
        public T Take(Vector3 position, Quaternion rotation)
        {
            var g = Take();
            g.transform.position = position;
            g.transform.rotation = rotation;
            return g;
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
                g.gameObject.SetActive(true);
            }
            else
            {
                g = new GameObject("Pooled Object", typeof(T)).GetComponent<T>();
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
            g.gameObject.SetActive(false);
            freeObjects.Add(g);
        }

        /// <summary>
        /// Place an object back into the pool after d seconds.
        /// </summary>
        /// <param name='g'>
        /// G.
        /// </param>
        /// <param name='d'>
        /// D.
        /// </param>
        public void Recycle(T g, float d)
        {
            g.StartCoroutine(_Free(g, d, null));
        }

        /// <summary>
        /// Place an object back into the pool after d seconds, then run an action.
        /// </summary>
        /// <param name='g'>
        /// G.
        /// </param>
        /// <param name='d'>
        /// D.
        /// </param>
        /// <param name='OnFree'>
        /// On free.
        /// </param>
        public void Recycle(T g, float d, System.Action OnFree)
        {
            g.StartCoroutine(_Free(g, d, OnFree));
        }

        IEnumerator _Free(T g, float d, System.Action OnFree)
        {
            yield return new WaitForSeconds(d);
            g.gameObject.SetActive(false);
            freeObjects.Add(g);
            if (OnFree != null)
                OnFree();
        }

    }
}



