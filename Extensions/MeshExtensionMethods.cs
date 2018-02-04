using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace DifferentMethods.Extensions
{
    public static class MeshExtensionMethods
    {
        /// <summary>
        /// Rescale a mesh uniformly.
        /// </summary>
        /// <param name="mesh">
        /// A <see cref="Mesh"/>
        /// </param>
        /// <param name="scale">
        /// A <see cref="System.Single"/>
        /// </param>
        public static void Scale(this Mesh mesh, float scale)
        {
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= scale;
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

        /// <summary>
        /// Rescale a mesh non-uniformly.
        /// </summary>
        /// <param name="mesh">
        /// A <see cref="Mesh"/>
        /// </param>
        /// <param name="scale">
        /// A <see cref="Vector3"/>
        /// </param>
        public static void Scale(this Mesh mesh, Vector3 scale)
        {
            var vertices = mesh.vertices;
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].x *= scale.x;
                vertices[i].y *= scale.y;
                vertices[i].z *= scale.z;
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

        /// <summary>
        /// Deform a mesh randomly by a scaled amount.
        /// </summary>
        /// <param name="mesh">
        /// A <see cref="Mesh"/>
        /// </param>
        /// <param name="scale">
        /// A <see cref="System.Single"/>
        /// </param>
        public static void Deform(this Mesh mesh, float scale)
        {
            var vertices = mesh.vertices;
            var normals = mesh.normals;

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] += (normals[i] * (Rnd.Value * scale));
            }
            mesh.vertices = vertices;
            mesh.RecalculateBounds();
        }

        /// <summary>
        /// Flip the normals of a mesh.
        /// </summary>
        /// <param name="mesh">
        /// A <see cref="Mesh"/>
        /// </param>
        public static void FlipNormals(this Mesh mesh)
        {
            var normals = mesh.normals;
            for (var i = 0; i < normals.Length; i++)
            {
                normals[i] *= -1;
            }
            mesh.normals = normals;
        }
    }
}