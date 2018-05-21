using UnityEngine;


public static class BoundsExtensionMethods
{
    public static Bounds Intersection(this Bounds A, Bounds B)
    {
        var min = new Vector3(Mathf.Max(A.min.x, B.min.x), Mathf.Max(A.min.y, B.min.y), Mathf.Max(A.min.z, B.min.z));
        var max = new Vector3(Mathf.Min(A.max.x, B.max.x), Mathf.Min(A.max.y, B.max.y), Mathf.Min(A.max.z, B.max.z));
        return new Bounds(Vector3.Lerp(min, max, 0.5f), max - min);
    }
}

