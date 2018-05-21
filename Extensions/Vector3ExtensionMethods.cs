using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class Vector3ExtensionMethods
{

    /// <summary>
    /// Shortcut to perform smooth step interpolation from one vector to another.
    /// </summary>
    public static Vector3 SmoothStep(this Vector3 from, Vector3 to, float t)
    {
        return Vector3.Lerp(from, to, Mathf.SmoothStep(0, 1, t));
    }

    public static bool Near(this Vector3 origin, Vector3 test, float maxDistance = 0.01f)
    {
        return (origin - test).sqrMagnitude < (maxDistance * maxDistance);
    }

    public static Vector3 Round(this Vector3 vector)
    {
        vector.x = Mathf.Round(vector.x);
        vector.y = Mathf.Round(vector.y);
        vector.z = Mathf.Round(vector.z);
        return vector;
    }

    public static Vector3 Floor(this Vector3 vector)
    {
        vector.x = Mathf.Floor(vector.x);
        vector.y = Mathf.Floor(vector.y);
        vector.z = Mathf.Floor(vector.z);
        return vector;
    }

    public static Vector3Int FloorToInt(this Vector3 vector)
    {
        var v = Vector3Int.zero;
        v.x = Mathf.FloorToInt(vector.x);
        v.y = Mathf.FloorToInt(vector.y);
        v.z = Mathf.FloorToInt(vector.z);
        return v;
    }

    public static Vector3 Abs(this Vector3 vector)
    {
        vector.x = Mathf.Abs(vector.x);
        vector.y = Mathf.Abs(vector.y);
        vector.z = Mathf.Abs(vector.z);
        return vector;
    }

    public static Vector2 xy(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.y);
    }

    public static Vector2 xz(this Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

    public static Vector2 yz(this Vector3 vector)
    {
        return new Vector2(vector.y, vector.z);
    }

}
