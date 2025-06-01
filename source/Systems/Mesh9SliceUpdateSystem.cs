using Collections.Generic;
using Meshes.Components;
using Meshes.NineSliced.Components;
using Simulation;
using System.Numerics;
using Transforms.Components;
using Worlds;
using Worlds.Messages;

namespace Meshes.NineSliced.Systems
{
    public partial class Mesh9SliceUpdateSystem : SystemBase, IListener<Update>
    {
        private readonly World world;
        private readonly Dictionary<Entity, Vector3> lastWorldScales;

        public Mesh9SliceUpdateSystem(Simulator simulator, World world) : base(simulator)
        {
            this.world = world;
            lastWorldScales = new(4);
        }

        public override void Dispose()
        {
            lastWorldScales.Dispose();
        }

        void IListener<Update>.Receive(ref Update message)
        {
            ComponentQuery<IsMesh, Mesh9SliceSettings, LocalToWorld> query = new(world);
            foreach (var r in query)
            {
                Entity entity = new(world, r.entity);
                bool changed;
                ref LocalToWorld ltw = ref r.component3;
                Vector3 worldScale = ltw.Scale;
                ref Vector3 lastWorldScale = ref lastWorldScales.TryGetValue(entity, out bool contains);
                if (contains)
                {
                    changed = lastWorldScale != worldScale;
                    lastWorldScale = worldScale;
                }
                else
                {
                    changed = true;
                    lastWorldScales.Add(entity, worldScale);
                }

                if (changed)
                {
                    ref Mesh9SliceSettings settings = ref r.component2;
                    entity.As<Mesh9Sliced>().UpdateVerticesAndUVs(settings.geometryMargins, settings.uvMargins, worldScale);
                }
            }
        }
    }
}