using System;
using System.Collections.Generic;
using UnityEngine;


public static class TextureExtensions
{
    public static Color[] GetPalette(this Texture2D sourceImage, int count = 4)
    {
        Color[] pixels;
        pixels = sourceImage.GetPixels();
        var palette = new List<Color>();
        var cuts = new Queue<Color[]>();
        cuts.Enqueue(pixels);
        var loops = (int)Mathf.Pow(2, count);
        while (cuts.Count < loops)
        {
            var p = cuts.Dequeue();
            Color[] top, bottom;
            ExtractColors(p, out top, out bottom);
            cuts.Enqueue(top);
            cuts.Enqueue(bottom);
        }

        while (cuts.Count > 0)
        {
            var cut = cuts.Dequeue();
            var color = (Vector4)Color.black;
            foreach (var i in cut)
                color += (Vector4)i;
            color /= cut.Length;
            color.w = 1;
            palette.Add(color);
        }
        return palette.ToArray();
    }

    static void ExtractColors(Color[] pixels, out Color[] top, out Color[] bottom)
    {
        var min = Color.white;
        var max = Color.black;
        foreach (var i in pixels)
        {
            min.r = Mathf.Min(min.r, i.r);
            min.g = Mathf.Min(min.g, i.g);
            min.b = Mathf.Min(min.b, i.b);
            max.r = Mathf.Min(max.r, i.r);
            max.g = Mathf.Min(max.g, i.g);
            max.b = Mathf.Min(max.b, i.b);
        }
        var range = max - min;
        var channel = 2;
        if (range.r >= range.g && range.r >= range.b)
            channel = 0;
        else if (range.g >= range.b)
            channel = 1;
        var keys = new float[pixels.Length];
        for (var i = 0; i < pixels.Length; i++)
            keys[i] = pixels[i][channel];
        System.Array.Sort(keys, pixels);
        var size = pixels.Length / 2;
        top = new Color[size];
        bottom = new Color[size];
        System.Array.Copy(pixels, 0, top, 0, size);
        System.Array.Copy(pixels, size, bottom, 0, size);
    }

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
