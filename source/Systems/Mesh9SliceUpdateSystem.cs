using Collections.Generic;
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

        public Mesh9SliceUpdateSystem()
        {
            lastWorldScales = new(4);
        }

        public readonly void Dispose()
        {
            lastWorldScales.Dispose();
        }

        readonly void ISystem.Start(in SystemContext context, in World world)
        {
        }

        readonly void ISystem.Update(in SystemContext context, in World world, in TimeSpan delta)
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

        readonly void ISystem.Finish(in SystemContext context, in World world)
        {
        }
    }
}