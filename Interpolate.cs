using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Convert linear 0.0f - 1.0f values to some other (non)linear value.
/// </summary>
public static class Interpolate
{
    public enum InterpolationType
    {
        Boing, Sine, Cosine, Cubic, Bounce, Qunitic, SmoothStep, Linear
    }

    public static float Boing(float v) => (Mathf.Sin(v * Mathf.PI * (0.2f + 2.5f * v * v * v)) * Mathf.Pow(1f - v, 2.2f) + v) * (1f + (1.2f * (1f - v)));


    public static float Sine(float v) => Mathf.Sin(v * Mathf.PI * 0.5f);


    public static float Cosine(float v) => (1.0f - Mathf.Cos(v * Mathf.PI * 0.5f));


    public static float Cubic(float v) => v * v * (3.0f - 2.0f * v);


    public static float Bounce(float v) => 1f - Mathf.Abs(Mathf.Sin((Mathf.PI * 1.5f) * (v + 1f) * (v + 1f)) * (1f - v));


    public static float Quintic(float v) => 6 * (v * v * v * v * v) - 15 * (v * v * v * v) + 10 * (v * v * v);


    public static float SmoothStep(float v) => Mathf.SmoothStep(0, 1, v);


    public static float Linear(float v) => Mathf.LerpUnclamped(0, 1, v);


    public static float Evaluate(InterpolationType type, float v)
    {
        switch (type)
        {
            case InterpolationType.Boing: return Boing(v);
            case InterpolationType.Bounce: return Bounce(v);
            case InterpolationType.Cosine: return Cosine(v);
            case InterpolationType.Cubic: return Cubic(v);
            case InterpolationType.Linear: return Linear(v);
            case InterpolationType.Qunitic: return Quintic(v);
            case InterpolationType.Sine: return Sine(v);
            case InterpolationType.SmoothStep: return SmoothStep(v);
        }
        return v;
    }
}