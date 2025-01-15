using Meshes.NineSliced.Components;
using Meshes.NineSliced.Systems;
using Meshes.Tests;
using Simulation;
using Simulation.Components;
using System.Numerics;
using Transforms;
using Transforms.Components;
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
            TypeLayout.Register<IsProgram>();
            TypeLayout.Register<IsTransform>();
            TypeLayout.Register<Position>();
            TypeLayout.Register<Rotation>();
            TypeLayout.Register<WorldRotation>();
            TypeLayout.Register<EulerAngles>();
            TypeLayout.Register<Scale>();
            TypeLayout.Register<Anchor>();
            TypeLayout.Register<Pivot>();
            TypeLayout.Register<LocalToWorld>();
            TypeLayout.Register<Mesh9SliceSettings>();
        }

        protected override void SetUp()
        {
            base.SetUp();
            world.Schema.RegisterTag<IsTransform>();
            world.Schema.RegisterComponent<IsProgram>();
            world.Schema.RegisterComponent<Position>();
            world.Schema.RegisterComponent<Rotation>();
            world.Schema.RegisterComponent<WorldRotation>();
            world.Schema.RegisterComponent<EulerAngles>();
            world.Schema.RegisterComponent<Scale>();
            world.Schema.RegisterComponent<Anchor>();
            world.Schema.RegisterComponent<Pivot>();
            world.Schema.RegisterComponent<LocalToWorld>();
            world.Schema.RegisterComponent<Mesh9SliceSettings>();
        }

        [Test]
        public void BuildDefaultSlicedMesh()
        {
            Mesh9Sliced mesh = new(world, new(0.5f, 0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f, 0.5f));
            Assert.That(mesh.GeometryMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));
            Assert.That(mesh.UVMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            uint vertexCount = mesh.GetVertexCount();
            USpan<Vector3> positions = mesh.GetVertexPositions();
            USpan<Vector2> uvs = mesh.GetVertexUVs();

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
            const float Third = 1f / 3f;
            Mesh9Sliced mesh = new(world, new(0.1f), new(Third));

            uint vertexCount = mesh.GetVertexCount();
            USpan<Vector3> positions = mesh.GetVertexPositions();
            USpan<Vector2> uvs = mesh.GetVertexUVs();

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
            using Simulator simulator = new(world);
            simulator.AddSystem<TransformSystem>();
            simulator.AddSystem<Mesh9SliceUpdateSystem>();

            Mesh9Sliced mesh = new(world, new(0.5f, 0.5f, 0.5f, 0.5f), new(0.5f, 0.5f, 0.5f, 0.5f));
            Transform meshTransform = mesh.AsEntity().Become<Transform>();
            meshTransform.LocalScale = new Vector3(4f, 2f, 2f);

            simulator.Update();

            Assert.That(mesh.GeometryMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));
            Assert.That(mesh.UVMargins, Is.EqualTo(new Vector4(0.5f, 0.5f, 0.5f, 0.5f)));

            uint vertexCount = mesh.GetVertexCount();
            USpan<Vector3> positions = mesh.GetVertexPositions();
            USpan<Vector2> uvs = mesh.GetVertexUVs();

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
