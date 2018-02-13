using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public static class UnityRectExtensions
{

    /// <summary>
    /// Test if this rect intersects with another rect.
    /// </summary>
    /// <param name="rect">
    /// A <see cref="Rect"/>
    /// </param>
    /// <param name="other">
    /// A <see cref="Rect"/>
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool Intersects(this Rect rect, Rect other)
    {
        if (rect.xMax < other.xMin)
            return false;
        if (rect.xMin > other.xMax)
            return false;
        if (rect.yMax < other.yMin)
            return false;
        if (rect.yMin > other.yMax)
            return false;
        return true;
    }

    /// <summary>
    /// Test if this rect intersects with any rects in the list.
    /// </summary>
    /// <param name="rect">
    /// A <see cref="Rect"/>
    /// </param>
    /// <param name="rects">
    /// A <see cref="IEnumerable(Rect)"/> The list of rects to check against.
    /// </param>
    /// <returns>
    /// A <see cref="System.Boolean"/>
    /// </returns>
    public static bool Intersects(this Rect rect, IEnumerable<Rect> rects)
    {
        foreach (var i in rects)
        {
            if (rect.Intersects(i))
                return true;
        }
        return false;
    }

    /// <summary>
    /// Convert the rect to an array of vertices.
    /// </summary>
    /// <param name="r">The rect.</param>
    public static Vector3[] Vertices(this Rect r, float grow = 0)
    {
        return new Vector3[] {
                new Vector2(r.xMin-grow, r.yMin-grow),
                new Vector2(r.xMin-grow, r.yMax+grow),
                new Vector2(r.xMax+grow, r.yMax+grow),
                new Vector2(r.xMax+grow, r.yMin-grow)
            };
    }


    public static Vector2 TopLeft(this Rect rect)
    {
        return new Vector2(rect.xMin, rect.yMin);
    }
    public static Rect ScaleSizeBy(this Rect rect, float scale)
    {
        return rect.ScaleSizeBy(scale, rect.center);
    }
    public static Rect ScaleSizeBy(this Rect rect, float scale, Vector2 pivotPoint)
    {
        Rect result = rect;
        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;
        result.xMin *= scale;
        result.xMax *= scale;
        result.yMin *= scale;
        result.yMax *= scale;
        result.x += pivotPoint.x;
        result.y += pivotPoint.y;
        return result;
    }

    public static Rect ScaleSizeBy(this Rect rect, Vector2 scale)
    {
        return rect.ScaleSizeBy(scale, rect.center);
    }

    public static Rect ScaleSizeBy(this Rect rect, Vector2 scale, Vector2 pivotPoint)
    {
        Rect result = rect;
        result.x -= pivotPoint.x;
        result.y -= pivotPoint.y;
        result.xMin *= scale.x;
        result.xMax *= scale.x;
        result.yMin *= scale.y;
        result.yMax *= scale.y;
        result.x += pivotPoint.x;
        result.y += pivotPoint.y;
        return result;
    }

    public static Rect Encapsulate(this Rect rect, Rect other)
    {
        rect.xMin = Mathf.Min(rect.xMin, other.xMin);
        rect.yMin = Mathf.Min(rect.yMin, other.yMin);
        rect.xMax = Mathf.Max(rect.xMax, other.xMax);
        rect.yMax = Mathf.Max(rect.yMax, other.yMax);
        return rect;
    }


}
