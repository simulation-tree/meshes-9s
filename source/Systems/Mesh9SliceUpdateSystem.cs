using Collections;
using Meshes.Components;
using Meshes.NineSliced.Components;
using Simulation;
using System;
using Transforms.Components;
using Worlds;

namespace Meshes.NineSliced.Systems
{
    public readonly partial struct Mesh9SliceUpdateSystem : ISystem
    {
        private readonly Dictionary<Entity, LocalToWorld> lastLtwPerEntity;

        private Mesh9SliceUpdateSystem(Dictionary<Entity, LocalToWorld> lastLtw)
        {
            this.lastLtwPerEntity = lastLtw;
        }

        void ISystem.Finish(in SystemContainer systemContainer, in World world)
        {
            if (systemContainer.World == world)
            {
                lastLtwPerEntity.Dispose();
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
                ref LocalToWorld lastLtw = ref lastLtwPerEntity.TryGetValue(entity, out bool contains);
                if (contains)
                {
                    changed = lastLtw != ltw;
                    lastLtw = ltw;
                }
                else
                {
                    changed = true;
                    lastLtwPerEntity.Add(entity, ltw);
                }

                if (changed)
                {
                    entity.As<Mesh9Sliced>().UpdateVerticesAndUVs();
                }
            }
        }
    }
}