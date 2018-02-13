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

}
