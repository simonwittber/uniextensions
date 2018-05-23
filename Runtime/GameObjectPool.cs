using System.Collections;
using System.Collections.Generic;
using DifferentMethods.Extensions.Collections;
using UnityEngine;


namespace DifferentMethods.Extensions
{
    public class GameObjectPool : MonoBehaviour
    {

        static GameObjectPool _Instance;

        public static GameObjectPool Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new GameObject("GameObjectPool", typeof(GameObjectPool)).GetComponent<GameObjectPool>();
                return _Instance;
            }
        }

        class PooledInstance : System.IComparable<PooledInstance>
        {
            public int refCount = 0;
            public GameObject gameObject;
            public Stack<PooledInstance> pool;
            public System.Action<GameObject> callback;
            public float lifetime;
            public int instanceKey;

            public int CompareTo(PooledInstance other)
            {
                return lifetime.CompareTo(other.lifetime);
            }
        }

        Dictionary<int, Stack<PooledInstance>> pools = new Dictionary<int, Stack<PooledInstance>>();
        Dictionary<int, PooledInstance> instances = new Dictionary<int, PooledInstance>();
        PriorityQueue<PooledInstance> pendingReturns = new PriorityQueue<PooledInstance>();

        static public void Prewarm(GameObject prefab, int count)
        {
            var pool = Instance.GetPool(prefab.GetInstanceID());
            for (var i = 0; i < count; i++)
                Instance.CreateInstance(prefab, pool);
        }

        static public T Take<T>(GameObject prefab, Transform matchTo = null, float lifetime = 0) where T : Component
        {
            var g = Take(prefab, matchTo, lifetime);
            var c = g.GetComponent<T>();
            if (c == null) c = g.AddComponent<T>();
            return c;
        }

        static public GameObject Take(GameObject prefab, Transform matchTo = null, float lifetime = 0)
        {
            var poolKey = prefab.GetInstanceID();
            var pool = Instance.GetPool(poolKey);
            PooledInstance p;
            if (pool.Count == 0)
                p = Instance.CreateInstance(prefab, pool);
            else
                p = pool.Pop();
            p.refCount++;
            if (lifetime > 0)
            {
                p.lifetime = Time.time + lifetime;
                Instance.pendingReturns.Push(p);
            }
            p.gameObject.SetActive(true);
            if (matchTo != null)
            {
                p.gameObject.transform.position = matchTo.position;
                p.gameObject.transform.rotation = matchTo.rotation;
            }
            return p.gameObject;
        }

        static public void OnReturn(GameObject go, System.Action<GameObject> callback)
        {
            Instance.instances[go.GetInstanceID()].callback = callback;
        }

        public static void Return(GameObject go, bool invokeCallback = true)
        {
            var instanceKey = go.GetInstanceID();
            PooledInstance pi;
            if (Instance.instances.TryGetValue(instanceKey, out pi))
                _Return(pi, invokeCallback);
            else
                Debug.LogError("Cannot return an instance that was not taken from a pool.");
        }

        static void _Return(PooledInstance pi, bool invokeCallback)
        {
            if (pi.refCount == 1)
            {
                pi.refCount--;
                if (invokeCallback && pi.callback != null)
                    pi.callback.Invoke(pi.gameObject);
                pi.callback = null;
                pi.gameObject.SetActive(false);
                Instance.pendingReturns.Remove(pi);
                pi.pool.Push(pi);
            }
        }

        PooledInstance CreateInstance(GameObject prefab, Stack<PooledInstance> pool)
        {
            var g = GameObject.Instantiate(prefab);
            g.SetActive(false);
            var p = new PooledInstance() { gameObject = g, pool = pool, instanceKey = g.GetInstanceID() };
            instances[p.instanceKey] = p;
            return p;
        }

        Stack<PooledInstance> GetPool(int key)
        {
            Stack<PooledInstance> pool;
            if (!pools.TryGetValue(key, out pool))
                pool = pools[key] = new Stack<PooledInstance>();
            return pool;
        }

        void Update()
        {
            while (pendingReturns.Count > 0 && pendingReturns.First.lifetime <= Time.time)
            {
                var pr = pendingReturns.Pop();
                _Return(pr, true);
            }
        }

        void Awake()
        {
            if (_Instance != null)
                DestroyImmediate(this);
            else
                _Instance = this;
        }

    }
}