using UnityEngine;

/// <summary>
/// A UniqueList is like a normal List, but contains only unique items
/// </summary>
namespace DifferentMethods.Extensions
{
    public static class ExtendedPrimitives
    {
        private const float HALFHEXRATIO = 0.4330127019f;

        static public Mesh Hexagon(float size, bool twoSides = true)
        {
            var A = HALFHEXRATIO;
            var mesh = new Mesh();

            mesh.vertices = new[] {
                new Vector3(0,0,0),
                new Vector3(-A, 0, -0.25f) * size,
                new Vector3(-A, 0, +0.25f) * size,
                new Vector3(0, 0, 0.5f) * size,
                new Vector3(+A, 0, +0.25f) * size,
                new Vector3(+A, 0, -0.25f) * size,
                new Vector3(0, 0, -0.5f) * size,
            };

            if (twoSides)
            {
                mesh.triangles = new[] {
                    0,1,2,
                    0,2,3,
                    0,3,4,
                    0,4,5,
                    0,6,1,
                    0,5,6,
                    0,2,1,
                    0,3,2,
                    0,4,3,
                    0,5,4   ,
                    0,1,6,
                    0,6,5
               };
            }
            else
            {
                mesh.triangles = new[] {
                    0,1,2,
                    0,2,3,
                    0,3,4,
                    0,4,5,
                    0,6,1,
                    0,5,6
               };

            }
            mesh.uv = new[] {
                new Vector2(0, 0) + Vector2.one*0.5f,
                new Vector2(-0.5f, 0) + Vector2.one*0.5f,
                new Vector2(-0.25f, A) + Vector2.one*0.5f,
                new Vector2(0.25f, A) + Vector2.one*0.5f,
                new Vector2(0.5f, 0) + Vector2.one*0.5f,
                new Vector2(0.25f, -A) + Vector2.one*0.5f,
                new Vector2(-0.25f, -A) + Vector2.one*0.5f
            };
            mesh.normals = new[] {
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0),
                new Vector3(0,1,0)
               };
            return mesh;
        }

        static public Mesh Quad(float size, bool twoSides = true)
        {
            var A = size / 2; ;
            var mesh = new Mesh();

            mesh.vertices = new[] {
                new Vector3(-A,0,-A),
                new Vector3(+A,0,-A),
                new Vector3(+A,0,+A),
                new Vector3(-A,0,+A),
            };

            if (twoSides)
            {
                mesh.triangles = new[] {
                    0,1,3,
                    1,2,3,
                    1,0,3,
                    2,1,3
               };
            }
            else
            {
                mesh.triangles = new[] {
                    0,1,3,
                    1,2,3
               };

            }
            mesh.uv = new[] {
                new Vector2(-0.5f, -0.5f),
                new Vector2(+0.5f, -0.5f),
                new Vector2(+0.5f, +0.5f),
                new Vector2(-0.5f, +0.5f)
            };
            mesh.RecalculateNormals();
            return mesh;
        }
    }
}