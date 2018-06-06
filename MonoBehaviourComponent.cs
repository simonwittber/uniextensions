using System;
using System.Collections;
using System.Collections.Generic;
using DifferentMethods.Extensions.Collections;
using UnityEngine;

namespace DifferentMethods.Extensions
{
    public abstract class MonoBehaviourSystem<T> : MonoBehaviour where T : MonoBehaviourComponent<T>
    {
        protected abstract void UpdateBatch(IList<T> components);

#if UNITY_EDITOR

        public int componentCount = 0;
        public double msPerUpdate = 0;
        System.Diagnostics.Stopwatch clock = new System.Diagnostics.Stopwatch();

        void Update()
        {
            clock.Start();
            UpdateBatch(ComponentList<T>.components);
            componentCount = ComponentList<T>.components.Count;
            clock.Stop();
            msPerUpdate = 1.0 * clock.ElapsedTicks / TimeSpan.TicksPerMillisecond;
            clock.Reset();
        }

#else

        void Update() => UpdateBatch(ComponentList<T>.components);

#endif

        void Awake()
        {
            if (ComponentList<T>.system != null)
                DestroyImmediate(this);
            else
                ComponentList<T>.system = this;
        }
    }

    static internal class ComponentList<T> where T : MonoBehaviourComponent<T>
    {
        internal static IList<T> components = new UnorderedList<T>();
        internal static MonoBehaviourSystem<T> system = null;

        internal static void Add(T component)
        {
            components.Add(component);
            if (system != null && !system.enabled)
                system.enabled = true;
        }

        internal static void Remove(T component)
        {
            components.Remove(component);
            if (components.Count == 0 && system != null && !system.enabled)
                system.enabled = false;
        }
    }

    public abstract class MonoBehaviourComponent<T> : MonoBehaviour where T : MonoBehaviourComponent<T>
    {
        protected virtual void OnEnable() => ComponentList<T>.Add((T)this);
        protected virtual void OnDisable() => ComponentList<T>.Remove((T)this);
    }
}