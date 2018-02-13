using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class GameObjectExtensionMethods
{

    /// <summary>
    /// Returns all child objects whose name contains query.
    /// </summary>
    /// <param name="component">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <param name="query">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="IEnumerable(GameObject)"/>
    /// </returns>
    public static IEnumerable<GameObject> FindChildren(this GameObject component, string query)
    {
        var children = new List<GameObject>();
        foreach (Transform t in component.transform)
        {
            if (t.name.Contains(query))
                children.Add(t.gameObject);
            children.AddRange(FindChildren(t.gameObject, query));
        }
        return children;
    }

    /// <summary>
    /// Find the child whose name exactly matches query.
    /// </summary>
    /// <param name="component">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <param name="query">
    /// A <see cref="System.String"/>
    /// </param>
    /// <returns>
    /// A <see cref="GameObject"/>
    /// </returns>
    public static Transform FindTransform(this GameObject component, string query)
    {
        foreach (Transform t in component.transform)
        {
            if (t.name == query)
                return t;
            else
            {
                var c = FindTransform(t.gameObject, query);
                if (c != null)
                    return c;
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the child who's name exactly manages query.
    /// </summary>
    /// <returns>
    /// The child.
    /// </returns>
    /// <param name='component'>
    /// Component.
    /// </param>
    /// <param name='query'>
    /// Query.
    /// </param>
    public static GameObject FindChild(this GameObject component, string query)
    {
        foreach (Transform t in component.transform)
        {
            if (t.name == query)
                return t.gameObject;
            else
            {
                var c = FindChild(t.gameObject, query);
                if (c != null)
                    return c;
            }
        }
        return null;
    }

    /// <summary>
    /// Gets the entire bounds of anything rendered under this game object.
    /// </summary>
    /// <returns>
    /// The bounds.
    /// </returns>
    /// <param name='g'>
    /// G.
    /// </param>
    public static Bounds GetBounds(this GameObject g)
    {
        Bounds b = new Bounds(Vector3.zero, Vector3.zero);
        var first = true;
        var components = g.GetComponentsInChildren<Renderer>();
        for (int j = 0, componentsLength = components.Length; j < componentsLength; j++)
        {
            var i = components[j];
            if (first)
                b = i.bounds;
            else
                b.Encapsulate(i.bounds);
            first = false;
        }
        return b;
    }

    /// <summary>
    /// Get all direct children of this game object.
    /// </summary>
    /// <param name="g">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <returns>
    /// A <see cref="GameObject[]"/>
    /// </returns>
    public static GameObject[] Children(this GameObject g)
    {
        var list = new List<GameObject>();
        foreach (Transform t in g.transform)
        {
            list.Add(t.gameObject);
        }
        return list.ToArray();
    }

    /// <summary>
    /// Set this gameobject and all children to a layer.
    /// </summary>
    /// <param name="g">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <param name="layer">
    /// A <see cref="System.Int32"/>
    /// </param>
    public static void SetLayer(this GameObject g, int layer)
    {
        g.layer = layer;
        foreach (Transform i in g.transform)
            SetLayer(i.gameObject, layer);
    }

    /// <summary>
    /// Get a component from a game object, and add it if it is missing.
    /// </summary>
    /// <param name="g">
    /// A <see cref="GameObject"/>
    /// </param>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public static T DefaultComponent<T>(this GameObject G) where T : Component
    {
        T c = G.GetComponent<T>();
        if (c == null)
            c = G.AddComponent<T>();
        return c;
    }

    /// <summary>
    /// Destroys the children.
    /// </summary>
    /// <param name="g">The gameobject.</param>
    public static void DestroyChildren(this GameObject go)
    {
        foreach (var g in go.Children())
        {
            GameObject.Destroy(g);
        }
    }

    /// <summary>
    /// Destroys the children immediately.
    /// </summary>
    /// <param name="g">The gameobject.</param>
    public static void DestroyChildrenImmediate(this GameObject go)
    {
        foreach (var g in go.Children())
        {
            GameObject.DestroyImmediate(g);
        }
    }
}
