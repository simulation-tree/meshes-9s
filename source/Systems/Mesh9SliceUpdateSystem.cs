using Collections;
using Meshes.Components;
using Meshes.NineSliced.Components;
using Simulation;
using System;
using System.Numerics;
using Transforms.Components;
using Worlds;

namespace Meshes.NineSliced.Systems
{
    public readonly partial struct Mesh9SliceUpdateSystem : ISystem
    {
        private readonly Dictionary<Entity, Vector3> lastWorldScales;

        private Mesh9SliceUpdateSystem(Dictionary<Entity, Vector3> lastLtw)
        {
            this.lastWorldScales = lastLtw;
        }

        void ISystem.Finish(in SystemContainer systemContainer, in World world)
        {
            if (systemContainer.World == world)
            {
                lastWorldScales.Dispose();
            }
        }

        void ISystem.Start(in SystemContainer systemContainer, in World world)
        {
            if (systemContainer.World == world)
            {
                systemContainer.Write(new Mesh9SliceUpdateSystem(new()));
            }
        }

        void ISystem.Update(in SystemContainer systemContainer, in World world, in TimeSpan delta)
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