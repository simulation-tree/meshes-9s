using Meshes.NineSliced.Systems;
using Simulation;
using Simulation.Tests;
using Transforms;
using Transforms.Messages;
using Transforms.Systems;
using Types;
using Worlds;
using Worlds.Messages;

namespace Meshes.NineSliced.Tests
{
    public abstract class SlicedMeshTests : SimulationTests
    {
        public World world;

        static SlicedMeshTests()
        {
            MetadataRegistry.Load<TransformsMetadataBank>();
            MetadataRegistry.Load<MeshesNineSlicedMetadataBank>();
            MetadataRegistry.Load<MeshesMetadataBank>();
        }

        protected override void SetUp()
        {
            base.SetUp();
            Schema schema = new();
            schema.Load<TransformsSchemaBank>();
            schema.Load<MeshesNineSlicedSchemaBank>();
            schema.Load<MeshesSchemaBank>();
            world = new(schema);
            Simulator.Add(new TransformSystem(Simulator, world));
            Simulator.Add(new Mesh9SliceUpdateSystem(Simulator, world));
        }

        protected override void TearDown()
        {
            Simulator.Remove<Mesh9SliceUpdateSystem>();
            Simulator.Remove<TransformSystem>();
            world.Dispose();
            base.TearDown();
        }

        protected override void Update(double deltaTime)
        {
            Simulator.Broadcast(new TransformUpdate());
            Simulator.Broadcast(new Update(deltaTime));
        }
    }
}