using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DifferentMethods.Extensions;

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

    /// <summary>
    /// Weld triangles together, using vertex position only (ignores colors, normals and uv).
    /// </summary>
    /// <param name="mesh"></param>
    public static void WeldVertices(this Mesh mesh)
    {
        var vertices = mesh.vertices;
        var unique = new List<Vector3>();
        var triangles = mesh.triangles;
        var map = new int[vertices.Length];
        for (var i = 0; i < vertices.Length; i++)
        {
            var v = vertices[i];
            var idx = unique.IndexOf(v);
            if (idx == -1)
            {
                map[i] = unique.Count;
                unique.Add(v);
            }
            else
            {
                map[i] = idx;
            }
        }
        for (var i = 0; i < triangles.Length; i++)
        {
            triangles[i] = map[triangles[i]];
        }
        mesh.Clear();
        mesh.SetVertices(unique);
        mesh.triangles = triangles;
    }

    /// <summary>
    /// Returns all vertex indices that are on an edge which is not shared between triangles.
    /// </summary>
    /// <param name="mesh"></param>
    /// <returns></returns>
    public static int[] GetEdgeVertices(this Mesh mesh)
    {
        var edges = new Dictionary<Edge, int>();
        var triangles = mesh.triangles;
        for (var i = 0; i < triangles.Length; i += 3)
        {
            var A = triangles[i + 0];
            var B = triangles[i + 1];
            var C = triangles[i + 2];

            foreach (var ea in new[] { new Edge(A, B), new Edge(B, C), new Edge(C, A) })
            {
                var count = 0;
                if (edges.TryGetValue(ea, out count))
                    edges[ea] = count + 1;
                else
                    edges[ea] = 1;
            }
        }
        var edgeVerts = new HashSet<int>();
        foreach (var i in edges)
        {
            if (i.Value == 1)
            {
                edgeVerts.Add(i.Key.A);
                edgeVerts.Add(i.Key.B);
            }
        }
        return edgeVerts.ToArray();
    }
}
