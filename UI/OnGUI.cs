using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions
{
    public class OnGUI
    {
        static float width;
        static float height;
        static float scale;
        static Vector3 offset;
        static List<Matrix4x4> stack = new List<Matrix4x4> ();
        public static Vector3 guiOffset = Vector3.zero;
    
        public static float AdjustedScale { 
            get { return scale; }
        }
    
        public static Vector2 Offset { 
            get { return new Vector2 (offset.x, offset.y); }
        }
    
        /// <summary>
        /// Resize all future GUI calls to fit within fixed dimensions.
        /// </summary>
        /// <param name="width">
        /// A <see cref="System.Single"/>
        /// </param>
        /// <param name="height">
        /// A <see cref="System.Single"/>
        /// </param>
        public static void Begin (float width, float height)
        {
            OnGUI.width = width;
            OnGUI.height = height;
            Push ();
            Matrix4x4 m = new Matrix4x4 ();
            ComputeScaleAndOffset ();
            m.SetTRS (offset + guiOffset, Quaternion.identity, Vector3.one * scale);
            GUI.matrix *= m;
        }
    
        /// <summary>
        /// Stop resizing the GUI. All future GUI calls will work as normal.
        /// </summary>
        public static void End ()
        {
            Pop ();
        }
    
        private static void ComputeScaleAndOffset ()
        {
            var w = (float)Screen.width;
            var h = (float)Screen.height;
            var aspect = w / h;
            scale = 1f;
            offset = Vector3.zero;
            if (aspect < (width / height)) {
                //screen is taller
                scale = (Screen.width / width);
                offset.y += (Screen.height - (height * scale)) * 0.5f;
            
            } else {
                // screen is wider
                scale = (Screen.height / height);
                offset.x += (Screen.width - (width * scale)) * 0.5f;
            }
        }

        public static void Push ()
        {
            stack.Add (GUI.matrix);
        }

        public static void Pop ()
        {
            GUI.matrix = stack [stack.Count - 1];
            stack.RemoveAt (stack.Count - 1);
        }

        public static void Translate (float x, float y)
        {
            Matrix4x4 m = new Matrix4x4 ();
            m.SetTRS (new Vector3 (x, y, 0), Quaternion.identity, Vector3.one);
            GUI.matrix *= m;
        }

        public static void Rotate (float a)
        {
            Matrix4x4 m = new Matrix4x4 ();
            m.SetTRS (Vector3.zero, Quaternion.Euler (0, 0, a), Vector3.one);
            GUI.matrix *= m;
        }

        public static void Scale (float s)
        {
            Matrix4x4 m = new Matrix4x4 ();
            m.SetTRS (Vector3.zero, Quaternion.identity, Vector3.one * s);
            GUI.matrix *= m;
        }
    }
}
