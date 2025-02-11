using Meshes.NineSliced.Components;
using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Transforms.Components;
using Unmanaged;
using Worlds;

namespace Meshes.NineSliced
{
    public readonly partial struct Mesh9Sliced : IEntity
    {
        private static readonly uint[] indices =
        [
            0, 1, 4, 4, 1, 5, 1, 2, 5, 5, 2, 6, 2, 3, 6, 6, 3, 7,
            4, 5, 8, 8, 5, 9, 5, 6, 9, 9, 6, 10, 6, 7, 10, 10, 7, 11,
            8, 9, 12, 12, 9, 13, 9, 10, 13, 13, 10, 14, 10, 11, 14, 14, 11, 15
        ];

        public readonly ref Vector4 GeometryMargins => ref GetComponent<Mesh9SliceSettings>().geometryMargins;
        public readonly ref Vector4 UVMargins => ref GetComponent<Mesh9SliceSettings>().uvMargins;

        [SkipLocalsInit]
        public Mesh9Sliced(World world, Vector4 geometryMargins, Vector4 uvMargins)
        {
            USpan<Vector4> colors = stackalloc Vector4[16];
            colors.Fill(new Vector4(1, 1, 1, 1));
            USpan<Vector3> vertices = stackalloc Vector3[16];
            USpan<Vector2> uvs = stackalloc Vector2[16];
            USpan<uint> indices = stackalloc uint[(16 * 3) + 6];
            CopyVerticesAndUVsTo(vertices, uvs, geometryMargins, uvMargins);
            CopyTrianglesTo(indices);

            this.world = world;
            value = new Mesh(world, vertices, uvs, colors, indices).value;
            AddComponent(new Mesh9SliceSettings(geometryMargins, uvMargins));
        }

        readonly void IEntity.Describe(ref Archetype archetype)
        {
            archetype.AddComponentType<Mesh9SliceSettings>();
        }

        /// <summary>
        /// Updates this 9 sliced mesh.
        /// </summary>
        public readonly void UpdateVerticesAndUVs()
        {
            Mesh mesh = As<Mesh>();
            ref LocalToWorld ltw = ref mesh.TryGetComponent<LocalToWorld>(out bool containsLtw);
            ref Mesh9SliceSettings settings = ref mesh.GetComponent<Mesh9SliceSettings>();
            Vector4 geometryMargins = settings.geometryMargins;
            Vector4 uvMargins = settings.uvMargins;
            if (containsLtw)
            {
                UpdateVerticesAndUVs(geometryMargins, uvMargins, ltw.Scale);
            }
            else
            {
                UpdateVerticesAndUVs(settings.geometryMargins, settings.uvMargins, Vector3.One);
            }
        }

        /// <summary>
        /// Updates this 9 sliced mesh to match the given margins with respect to scale.
        /// </summary>
        public readonly void UpdateVerticesAndUVs(Vector4 geometryMargins, Vector4 uvMargins, Vector3 worldScale)
        {
            geometryMargins.X /= worldScale.X;
            geometryMargins.Y /= worldScale.X;
            geometryMargins.Z /= worldScale.Y;
            geometryMargins.W /= worldScale.Y;

            Mesh mesh = As<Mesh>();
            USpan<Vector3> vertices = mesh.Positions;
            USpan<Vector2> uvs = mesh.UVs;
            CopyVerticesAndUVsTo(vertices, uvs, geometryMargins, uvMargins);
            mesh.IncrementVersion();
        }

        public static void CopyVerticesAndUVsTo(USpan<Vector3> vertices, USpan<Vector2> uvs, Vector4 geometryMargins, Vector4 uvMargins)
        {
            USpan<float> xVertexValues = [0, geometryMargins.X, 1 - geometryMargins.Y, 1];
            USpan<float> yVertexValues = [0, geometryMargins.Z, 1 - geometryMargins.W, 1];
            USpan<float> xUVValues = [0, uvMargins.X, 1 - uvMargins.Y, 1];
            USpan<float> yUVValues = [0, uvMargins.Z, 1 - uvMargins.W, 1];
            for (uint vertexIndex = 0; vertexIndex < 16; vertexIndex++)
            {
                uint x = vertexIndex % 4;
                uint y = vertexIndex / 4;
                vertices[vertexIndex] = new(xVertexValues[x], yVertexValues[y], 0);
                uvs[vertexIndex] = new(xUVValues[x], yUVValues[y]);
            }
        }

        public static uint CopyTrianglesTo(USpan<uint> triangles)
        {
            indices.CopyTo(triangles);
            return (uint)indices.Length;
        }

        public static implicit operator Mesh(Mesh9Sliced mesh9Sliced)
        {
            return mesh9Sliced.As<Mesh>();
        }
    }
}