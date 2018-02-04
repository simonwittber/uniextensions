using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DifferentMethods.Extensions
{
    /// <summary>
    /// Convert linear 0.0f - 1.0f values to some other non-linear value.
    /// </summary>
    public static class Interpolation
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Boing(float v) => (Mathf.Sin(v * Mathf.PI * (0.2f + 2.5f * v * v * v)) * Mathf.Pow(1f - v, 2.2f) + v) * (1f + (1.2f * (1f - v)));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Sine(float v) => Mathf.Sin(v * Mathf.PI * 0.5f);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cosine(float v) => (1.0f - Mathf.Cos(v * Mathf.PI * 0.5f));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Cubic(float v) => v * v * (3.0f - 2.0f * v);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Bounce(float v) => 1f - Mathf.Abs(Mathf.Sin((Mathf.PI * 1.5f) * (v + 1f) * (v + 1f)) * (1f - v));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Quintic(float v) => 6 * (v * v * v * v * v) - 15 * (v * v * v * v) + 10 * (v * v * v);
    }
}