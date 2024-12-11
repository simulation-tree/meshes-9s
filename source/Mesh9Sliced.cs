using Meshes.NineSliced.Components;
using System;
using System.Numerics;
using Transforms.Components;
using Unmanaged;
using Worlds;

namespace Meshes.NineSliced
{
    public readonly struct Mesh9Sliced : IMesh
    {
        private static readonly uint[] indices =
        [
            0, 1, 4, 4, 1, 5, 1, 2, 5, 5, 2, 6, 2, 3, 6, 6, 3, 7,
            4, 5, 8, 8, 5, 9, 5, 6, 9, 9, 6, 10, 6, 7, 10, 10, 7, 11,
            8, 9, 12, 12, 9, 13, 9, 10, 13, 13, 10, 14, 10, 11, 14, 14, 11, 15
        ];

        private readonly Mesh mesh;

        public readonly ref Vector4 GeometryMargins => ref mesh.AsEntity().GetComponent<Mesh9SliceSettings>().geometryMargins;
        public readonly ref Vector4 UVMargins => ref mesh.AsEntity().GetComponent<Mesh9SliceSettings>().uvMargins;

        readonly uint IEntity.Value => mesh.GetEntityValue();
        readonly World IEntity.World => mesh.GetWorld();
        readonly Definition IEntity.Definition => Definition.Get<Mesh>().AddComponentType<Mesh9SliceSettings>();

#if NET
        [Obsolete("Default constructor not supported", true)]
        public Mesh9Sliced()
        {
            throw new NotSupportedException();
        }
#endif
        public Mesh9Sliced(World world, Vector4 geometryMargins, Vector4 uvMargins)
        {
            mesh = new Mesh(world);
            USpan<Vector4> colors = mesh.CreateColors(16);
            for (uint i = 0; i < 16; i++)
            {
                colors[i] = new Vector4(1, 1, 1, 1);
            }

            USpan<Vector3> vertices = mesh.CreatePositions(16);
            USpan<Vector2> uvs = mesh.CreateUVs(16);
            GetVerticesAndUVs(vertices, uvs, geometryMargins, uvMargins);
            USpan<uint> indices = mesh.ResizeIndices((16 * 3) + 6);
            GetTriangles(indices);
            mesh.AsEntity().AddComponent(new Mesh9SliceSettings(geometryMargins, uvMargins));
        }

        public readonly void Dispose()
        {
            mesh.Dispose();
        }

        /// <summary>
        /// Updates this 9 sliced mesh.
        /// </summary>
        public readonly void UpdateVerticesAndUVs()
        {
            ref LocalToWorld ltw = ref mesh.AsEntity().TryGetComponent<LocalToWorld>(out bool contains);
            ref Mesh9SliceSettings settings = ref mesh.AsEntity().GetComponent<Mesh9SliceSettings>();
            Vector4 geometryMargins = settings.geometryMargins;
            Vector4 uvMargins = settings.uvMargins;
            if (contains)
            {
                Vector3 worldScale = ltw.Scale;
                geometryMargins.X /= worldScale.X;
                geometryMargins.Y /= worldScale.X;
                geometryMargins.Z /= worldScale.Y;
                geometryMargins.W /= worldScale.Y;
            }

            USpan<Vector3> vertices = mesh.GetVertexPositions();
            USpan<Vector2> uvs = mesh.GetVertexUVs();
            GetVerticesAndUVs(vertices, uvs, geometryMargins, uvMargins);
            mesh.IncrementVersion();
        }

        public static void GetVerticesAndUVs(USpan<Vector3> vertices, USpan<Vector2> uvs, Vector4 geometryMargins, Vector4 uvMargins)
        {
            float geometryLeft = geometryMargins.X;
            float geometryRight = geometryMargins.Y;
            float geometryTop = geometryMargins.Z;
            float geometryBottom = geometryMargins.W;
            float uvLeft = uvMargins.X;
            float uvRight = uvMargins.Y;
            float uvTop = uvMargins.Z;
            float uvBottom = uvMargins.W;
            for (uint vertexIndex = 0; vertexIndex < 16; vertexIndex++)
            {
                uint x = vertexIndex % 4;
                uint y = vertexIndex / 4;
                Vector3 vertex = default;
                Vector2 uv = default;

                if (x == 0)
                {
                    vertex.X = 0;
                    uv.X = 0;
                }
                else if (x == 1)
                {
                    vertex.X = geometryLeft;
                    uv.X = uvLeft;
                }
                else if (x == 2)
                {
                    vertex.X = 1 - geometryRight;
                    uv.X = 1 - uvRight;
                }
                else
                {
                    vertex.X = 1;
                    uv.X = 1;
                }

                if (y == 0)
                {
                    vertex.Y = 0;
                    uv.Y = 0;
                }
                else if (y == 1)
                {
                    vertex.Y = geometryTop;
                    uv.Y = uvTop;
                }
                else if (y == 2)
                {
                    vertex.Y = 1 - geometryBottom;
                    uv.Y = 1 - uvBottom;
                }
                else if (y == 3)
                {
                    vertex.Y = 1;
                    uv.Y = 1;
                }

                vertices[vertexIndex] = vertex;
                uvs[vertexIndex] = uv;
            }
        }

        public static void GetTriangles(USpan<uint> triangles)
        {
            indices.AsUSpan().CopyTo(triangles);
        }

        public static implicit operator Entity(Mesh9Sliced mesh9Sliced)
        {
            return mesh9Sliced.mesh;
        }

        public static implicit operator Mesh(Mesh9Sliced mesh9Sliced)
        {
            return mesh9Sliced.mesh;
        }
    }
}