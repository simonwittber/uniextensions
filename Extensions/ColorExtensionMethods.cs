using UnityEngine;
using System.Collections;


public static class ColorExtensionMethods
{

    public static DifferentMethods.Extensions.Colors.HSV ToHSV(this Color color)
    {
        var r = color.r;
        var g = color.g;
        var b = color.b;

        float h, s, v;

        float min, max, delta;
        min = Mathf.Min(r, g, b);
        max = Mathf.Max(r, g, b);
        v = max;
        delta = max - min;
        if (max != 0)
            s = delta / max;
        else
        {
            s = 0;
            h = -1;
            return new DifferentMethods.Extensions.Colors.HSV(h, s, v, color.a);
        }
        if (r == max)
            h = (g - b) / delta;
        else if (g == max)
            h = 2 + (b - r) / delta;
        else
            h = 4 + (r - g) / delta;
        h *= 60;
        if (h < 0)
            h += 360;
        return new DifferentMethods.Extensions.Colors.HSV(h, s, v, color.a);
    }


}
