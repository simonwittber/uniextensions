using System;
using UnityEngine;


public static class TextureExtensions
{
    public static void Blur(this Texture2D texture, int iterations)
    {
        var r = new float[texture.width, texture.height];
        var g = new float[texture.width, texture.height];
        var b = new float[texture.width, texture.height];
        var a = new float[texture.width, texture.height];
        var pixels = texture.GetPixels();
        for (var x = 0; x < texture.width; x++)
        {
            for (var y = 0; y < texture.height; y++)
            {
                var pixel = pixels[y * texture.width + x];
                r[x, y] = pixel.r;
                g[x, y] = pixel.g;
                b[x, y] = pixel.b;
                a[x, y] = pixel.a;
            }
        }
        for (var i = 0; i < iterations; i++)
        {
            r = Blur(r, texture.width, texture.height);
            g = Blur(g, texture.width, texture.height);
            b = Blur(b, texture.width, texture.height);
            a = Blur(a, texture.width, texture.height);
        }
        for (var x = 0; x < texture.width; x++)
        {
            for (var y = 0; y < texture.height; y++)
            {
                pixels[y * texture.width + x] = new Color(r[x, y], g[x, y], b[x, y], a[x, y]);
            }
        }
        texture.SetPixels(pixels);
        texture.Apply();
    }

    static float[,] Blur(float[,] src, int w, int h)
    {
        var dst = src.Clone() as float[,];
        for (var x = 0; x < w; x++)
        {
            for (var y = 0; y < h; y++)
            {
                //(1 2 1)
                //(2 4 2) * 1/16
                //(1 2 1)
                var left = x - 1 < 0 ? 0 : x - 1;
                var right = x + 1 >= w ? w - 1 : x + 1;
                var top = y - 1 < 0 ? 0 : y - 1;
                var bottom = y + 1 >= h ? h - 1 : y + 1;

                var d = src[left, top] + src[left, bottom] + src[right, top] + src[right, bottom] + (2 * src[left, y]) + (2 * src[right, y]) + (2 * src[x, top]) + (2 * src[x, bottom]) + (4 * src[x, y]);
                d *= (1f / 16f);
                dst[x, y] = d;
            }
        }
        return dst;
    }
}
