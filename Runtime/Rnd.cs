using UnityEngine;
using System.Collections;

namespace DifferentMethods.Extensions
{
    /* These methods derived from Python 2.7 standard lib. */
    public class Rnd
    {
        public static System.Random rnd = new System.Random();
        /// <summary>
        /// Triangular distribution.
        /// Continuous distribution bounded by given lower and upper limits,
        /// and having a given mode value in-between.
        /// http://en.wikipedia.org/wiki/Triangular_distribution
        /// </summary>
        public static float Triangular(float low = 0f, float high = 1f, float? mode = null)
        {
            var u = Value;
            var c = mode.HasValue ? (mode.Value - low) / (high - low) : 0.5f;
            if (u > c)
            {
                u = 1f - u;
                c = 1f - c;
                var t = low;
                low = high;
                high = t;
            }
            return Mathf.Pow(low + (high - low) * (u * c), 0.5f);
        }

        /// <summary>
        /// mu is the mean, and sigma is the standard deviation.
        /// </summary>
        public static float Gauss(float mu, float sigma)
        {
            var z = gauss_next;
            gauss_next = null;
            if (!z.HasValue)
            {
                var x2pi = Value * Mathf.PI * 2f;
                var g2rad = Mathf.Sqrt(-2f * Mathf.Log(1f - Value));
                z = Mathf.Cos(x2pi) * g2rad;
                gauss_next = Mathf.Sin(x2pi) * g2rad;
            }
            return mu + z.Value * sigma;
        }

        static float? gauss_next;

        public static float Value
        {
            get
            {
                return (float)rnd.NextDouble();
            }
        }

        public static float Range(float min, float max)
        {
            return ((max - min) * Value) + min;
        }

        public static int Range(int min, int max)
        {
            return (int)(((max - min) * Value) + min);
        }
    }

}




