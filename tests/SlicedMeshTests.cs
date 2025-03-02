using Meshes.NineSliced.Systems;
using Meshes.Tests;
using Simulation;
using System.Numerics;
using Transforms;
using Transforms.Systems;
using Types;
using Unmanaged;
using Worlds;

namespace Meshes.NineSliced.Tests
{
    public class SlicedMeshTests : MeshTests
    {
        static SlicedMeshTests()
        {
            TypeRegistry.Load<SimulationTypeBank>();
            TypeRegistry.Load<TransformsTypeBank>();
            TypeRegistry.Load<MeshesNineSlicedTypeBank>();
        }

        protected override Schema CreateSchema()
        {
            Schema schema = base.CreateSchema();
            schema.Load<SimulationSchemaBank>();
            schema.Load<TransformsSchemaBank>();
            schema.Load<MeshesNineSlicedSchemaBank>();
            return schema;
        }

        [Test]
        public void BuildDefaultSlicedMesh()
        {
            using World world = CreateWorld();
            Mesh9Sliced slicedMesh = new(world, new(0.5f, 0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.That(slicedMesh.GeometryMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));
            Assert.That(slicedMesh.UVMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            Mesh mesh = slicedMesh;
            uint vertexCount = mesh.VertexCount;
            Mesh.Collection<Vector3> positions = mesh.Positions;
            Mesh.Collection<Vector2> uvs = mesh.UVs;

            //first row
            AssertVectorEquality(positions[0], new Vector3(0f, 0f, 0f));
            AssertVectorEquality(positions[1], new Vector3(0.5f, 0f, 0f));
            AssertVectorEquality(positions[2], new Vector3(0.5f, 0f, 0f));
            AssertVectorEquality(positions[3], new Vector3(1f, 0f, 0f));
            AssertVectorEquality(uvs[0], new Vector2(0f, 0f));
            AssertVectorEquality(uvs[1], new Vector2(0.5f, 0f));
            AssertVectorEquality(uvs[2], new Vector2(0.5f, 0f));
            AssertVectorEquality(uvs[3], new Vector2(1f, 0f));

            //second row
            AssertVectorEquality(positions[4], new Vector3(0f, 0.5f, 0f));
            AssertVectorEquality(positions[5], new Vector3(0.5f, 0.5f, 0f));
            AssertVectorEquality(positions[6], new Vector3(0.5f, 0.5f, 0f));
            AssertVectorEquality(positions[7], new Vector3(1f, 0.5f, 0f));
            AssertVectorEquality(uvs[4], new Vector2(0f, 0.5f));
            AssertVectorEquality(uvs[5], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[6], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[7], new Vector2(1f, 0.5f));

            //third row
            AssertVectorEquality(positions[8], new Vector3(0f, 0.5f, 0f));
            AssertVectorEquality(positions[9], new Vector3(0.5f, 0.5f, 0f));
            AssertVectorEquality(positions[10], new Vector3(0.5f, 0.5f, 0f));
            AssertVectorEquality(positions[11], new Vector3(1f, 0.5f, 0f));
            AssertVectorEquality(uvs[8], new Vector2(0f, 0.5f));
            AssertVectorEquality(uvs[9], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[10], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[11], new Vector2(1f, 0.5f));

            //fourth row
            AssertVectorEquality(positions[12], new Vector3(0f, 1f, 0f));
            AssertVectorEquality(positions[13], new Vector3(0.5f, 1f, 0f));
            AssertVectorEquality(positions[14], new Vector3(0.5f, 1f, 0f));
            AssertVectorEquality(positions[15], new Vector3(1f, 1f, 0f));
            AssertVectorEquality(uvs[12], new Vector2(0f, 1f));
            AssertVectorEquality(uvs[13], new Vector2(0.5f, 1f));
            AssertVectorEquality(uvs[14], new Vector2(0.5f, 1f));
            AssertVectorEquality(uvs[15], new Vector2(1f, 1f));
        }

        [Test]
        public void BuildSubtleSlicedMesh()
        {
            using World world = CreateWorld();
            const float Third = 1f / 3f;
            Mesh mesh = new Mesh9Sliced(world, new(0.1f), new(Third));

            uint vertexCount = mesh.VertexCount;
            Mesh.Collection<Vector3> positions = mesh.Positions;
            Mesh.Collection<Vector2> uvs = mesh.UVs;

            //first row
            AssertVectorEquality(positions[0], new Vector3(0f, 0f, 0f));
            AssertVectorEquality(positions[1], new Vector3(0.1f, 0f, 0f));
            AssertVectorEquality(positions[2], new Vector3(0.9f, 0f, 0f));
            AssertVectorEquality(positions[3], new Vector3(1f, 0f, 0f));
            AssertVectorEquality(uvs[0], new Vector2(Third * 0, Third * 0));
            AssertVectorEquality(uvs[1], new Vector2(Third * 1, Third * 0));
            AssertVectorEquality(uvs[2], new Vector2(Third * 2, Third * 0));
            AssertVectorEquality(uvs[3], new Vector2(Third * 3, Third * 0));

            //second row
            AssertVectorEquality(positions[4], new Vector3(0f, 0.1f, 0f));
            AssertVectorEquality(positions[5], new Vector3(0.1f, 0.1f, 0f));
            AssertVectorEquality(positions[6], new Vector3(0.9f, 0.1f, 0f));
            AssertVectorEquality(positions[7], new Vector3(1f, 0.1f, 0f));
            AssertVectorEquality(uvs[4], new Vector2(Third * 0, Third * 1));
            AssertVectorEquality(uvs[5], new Vector2(Third * 1, Third * 1));
            AssertVectorEquality(uvs[6], new Vector2(Third * 2, Third * 1));
            AssertVectorEquality(uvs[7], new Vector2(Third * 3, Third * 1));

            //third row
            AssertVectorEquality(positions[8], new Vector3(0f, 0.9f, 0f));
            AssertVectorEquality(positions[9], new Vector3(0.1f, 0.9f, 0f));
            AssertVectorEquality(positions[10], new Vector3(0.9f, 0.9f, 0f));
            AssertVectorEquality(positions[11], new Vector3(1f, 0.9f, 0f));
            AssertVectorEquality(uvs[8], new Vector2(Third * 0, Third * 2));
            AssertVectorEquality(uvs[9], new Vector2(Third * 1, Third * 2));
            AssertVectorEquality(uvs[10], new Vector2(Third * 2, Third * 2));
            AssertVectorEquality(uvs[11], new Vector2(Third * 3, Third * 2));

            //fourth row
            AssertVectorEquality(positions[12], new Vector3(0f, 1f, 0f));
            AssertVectorEquality(positions[13], new Vector3(0.1f, 1f, 0f));
            AssertVectorEquality(positions[14], new Vector3(0.9f, 1f, 0f));
            AssertVectorEquality(positions[15], new Vector3(1f, 1f, 0f));
            AssertVectorEquality(uvs[12], new Vector2(Third * 0, Third * 3));
            AssertVectorEquality(uvs[13], new Vector2(Third * 1, Third * 3));
            AssertVectorEquality(uvs[14], new Vector2(Third * 2, Third * 3));
            AssertVectorEquality(uvs[15], new Vector2(Third * 3, Third * 3));
        }

        [Test]
        public void ScaledDefaultSlicedMesh()
        {
            using World world = CreateWorld();
            using Simulator simulator = new(world);
            simulator.AddSystem<TransformSystem>();
            simulator.AddSystem<Mesh9SliceUpdateSystem>();

            Mesh9Sliced slicedMesh = new(world, new(0.5f, 0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f, 0.5f));
            Transform meshTransform = slicedMesh.Become<Transform>();
            meshTransform.LocalScale = new Vector3(4f, 2f, 2f);

            simulator.Update();

            Assert.That(slicedMesh.GeometryMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));
            Assert.That(slicedMesh.UVMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            Mesh mesh = slicedMesh;
            uint vertexCount = mesh.VertexCount;
            Mesh.Collection<Vector3> positions = mesh.Positions;
            Mesh.Collection<Vector2> uvs = mesh.UVs;

            AssertVectorEquality(positions[0], new Vector3(0f, 0f, 0f));
            AssertVectorEquality(positions[1], new Vector3(0.125f, 0f, 0f));
            AssertVectorEquality(positions[2], new Vector3(0.875f, 0f, 0f));
            AssertVectorEquality(positions[3], new Vector3(1f, 0f, 0f));
            AssertVectorEquality(uvs[0], new Vector2(0f, 0f));
            AssertVectorEquality(uvs[1], new Vector2(0.5f, 0f));
            AssertVectorEquality(uvs[2], new Vector2(0.5f, 0f));
            AssertVectorEquality(uvs[3], new Vector2(1f, 0f));

            AssertVectorEquality(positions[4], new Vector3(0f, 0.25f, 0f));
            AssertVectorEquality(positions[5], new Vector3(0.125f, 0.25f, 0f));
            AssertVectorEquality(positions[6], new Vector3(0.875f, 0.25f, 0f));
            AssertVectorEquality(positions[7], new Vector3(1f, 0.25f, 0f));
            AssertVectorEquality(uvs[4], new Vector2(0f, 0.5f));
            AssertVectorEquality(uvs[5], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[6], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[7], new Vector2(1f, 0.5f));

            AssertVectorEquality(positions[8], new Vector3(0f, 0.75f, 0f));
            AssertVectorEquality(positions[9], new Vector3(0.125f, 0.75f, 0f));
            AssertVectorEquality(positions[10], new Vector3(0.875f, 0.75f, 0f));
            AssertVectorEquality(positions[11], new Vector3(1f, 0.75f, 0f));
            AssertVectorEquality(uvs[8], new Vector2(0f, 0.5f));
            AssertVectorEquality(uvs[9], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[10], new Vector2(0.5f, 0.5f));
            AssertVectorEquality(uvs[11], new Vector2(1f, 0.5f));

            AssertVectorEquality(positions[12], new Vector3(0f, 1f, 0f));
            AssertVectorEquality(positions[13], new Vector3(0.125f, 1f, 0f));
            AssertVectorEquality(positions[14], new Vector3(0.875f, 1f, 0f));
            AssertVectorEquality(positions[15], new Vector3(1f, 1f, 0f));
            AssertVectorEquality(uvs[12], new Vector2(0f, 1f));
            AssertVectorEquality(uvs[13], new Vector2(0.5f, 1f));
            AssertVectorEquality(uvs[14], new Vector2(0.5f, 1f));
            AssertVectorEquality(uvs[15], new Vector2(1f, 1f));
        }

        private static void AssertVectorEquality(Vector3 actual, Vector3 expected)
        {
            Vector3 within = new(0.001f);
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(within.X));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(within.Y));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(within.Z));
        }

        private static void AssertVectorEquality(Vector2 actual, Vector2 expected)
        {
            Vector2 within = new(0.001f);
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(within.X));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(within.Y));
        }
    }
}