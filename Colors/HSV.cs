using UnityEngine;
using System.Collections;

namespace DifferentMethods.Extensions.Colors
{
    /// <summary>
    /// Represents a color using HSV color space.
    /// </summary>
    public struct HSV
    {
        public float H, S, V;
        public float A;

        public HSV(float H, float S, float V)
        {
            this.H = H;
            this.S = S;
            this.V = V;
            this.A = 1f;
        }

        public HSV(float H, float S, float V, float A)
        {
            this.H = H;
            this.S = S;
            this.V = V;
            this.A = A;
        }
        /// <summary>
        /// Convert to normal RGB Color.
        /// </summary>
        /// <returns>UnityEngine.Color</returns>
        public Color ToRGB()
        {
            var color = new Color(1, 1, 1, 1);
            if (S == 0f)
            {
                color.r = V;
                color.g = V;
                color.b = V;
            }
            else
            {
                if (V == 0f)
                {
                    color.r = 0f;
                    color.g = 0f;
                    color.b = 0f;
                }
                else
                {
                    color.r = 0f;
                    color.g = 0f;
                    color.b = 0f;
                    var I = H * 6f;
                    var B = Mathf.FloorToInt(I);
                    var C = I - (float)B;
                    var D = V * (1f - S);
                    var E = V * (1f - S * C);
                    var F = V * (1f - S * (1f - C));
                    var G = B;
                    switch (G + 1)
                    {
                        case 0:
                            color.r = V;
                            color.g = D;
                            color.b = E;
                            break;
                        case 1:
                            color.r = V;
                            color.g = F;
                            color.b = D;
                            break;
                        case 2:
                            color.r = E;
                            color.g = V;
                            color.b = D;
                            break;
                        case 3:
                            color.r = D;
                            color.g = V;
                            color.b = F;
                            break;
                        case 4:
                            color.r = D;
                            color.g = E;
                            color.b = V;
                            break;
                        case 5:
                            color.r = F;
                            color.g = D;
                            color.b = V;
                            break;
                        case 6:
                            color.r = V;
                            color.g = D;
                            color.b = E;
                            break;
                        case 7:
                            color.r = V;
                            color.g = F;
                            color.b = D;
                            break;
                    }
                    color.r = Mathf.Clamp(color.r, 0f, 1f);
                    color.g = Mathf.Clamp(color.g, 0f, 1f);
                    color.b = Mathf.Clamp(color.b, 0f, 1f);
                    color.a = Mathf.Clamp(A, 0f, 1f);
                }
            }
            return color;
        }

    }
}
