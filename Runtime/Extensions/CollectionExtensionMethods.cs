using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DifferentMethods.Extensions;

public static class CollectionExtensionMethods
{
    /// <summary>
    /// Shuffle the specified list.
    /// </summary>
    /// <param name="list">List.</param>
    /// <typeparam name="T">The 1st type parameter.</typeparam>
    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Mathf.FloorToInt(Rnd.Value * (n + 1));
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    /// <summary>
    /// Get a random element from the list.
    /// </summary>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public static T GetRandom<T>(this IList<T> list)
    {
        return list[Rnd.Range(0, list.Count)];
    }

    /// <summary>
    /// Remove and return a random element from the list.
    /// </summary>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public static T PopRandom<T>(this IList<T> list)
    {
        var i = Rnd.Range(0, list.Count);
        var o = list[i];
        list.RemoveAt(i);
        return o;
    }

    /// <summary>
    /// Remove and return a element in the list by index. Out of range indices are wrapped into range.
    /// </summary>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public static T Pop<T>(this IList<T> list, int index)
    {
        while (index > list.Count)
            index -= list.Count;
        while (index < 0)
            index += list.Count;
        var o = list[index];
        list.RemoveAt(index);
        return o;
    }

    /// <summary>
    /// Return an element from a list by index. Out of range indices are wrapped into range.
    /// </summary>
    /// <returns>
    /// A <see cref="T"/>
    /// </returns>
    public static T Get<T>(this IList<T> list, int index)
    {
        while (index > list.Count)
            index -= list.Count;
        while (index < 0)
            index += list.Count;
        return list[index];
    }

    /// <summary>
    /// Remove and return a value from a dictionary.
    /// </summary>
    /// <param name='dict'>
    /// Dict.
    /// </param>
    /// <param name='key'>
    /// Key.
    /// </param>
    /// <typeparam name='TKey'>
    /// The 1st type parameter.
    /// </typeparam>
    /// <typeparam name='TValue'>
    /// The 2nd type parameter.
    /// </typeparam>
    public static TValue Pop<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
    {
        var item = dict[key];
        dict.Remove(key);
        return item;
    }

    public static void Map<T>(this IList<T> list, System.Action<T> fn)
    {
        for (var i = 0; i < list.Count; i++)
        {
            fn(list[i]);
        }
    }

    public static void Map<T>(this T[] list, System.Action<T> fn)
    {
        for (var i = 0; i < list.Length; i++)
        {
            fn(list[i]);
        }
    }

    public static void Apply<T>(this IList<T> list, System.Func<T, T> fn)
    {
        for (var i = 0; i < list.Count; i++)
        {
            list[i] = fn(list[i]);
        }
    }

    public static void Apply<T>(this T[] list, System.Func<T, T> fn)
    {
        for (var i = 0; i < list.Length; i++)
        {
            list[i] = fn(list[i]);
        }
    }

}
