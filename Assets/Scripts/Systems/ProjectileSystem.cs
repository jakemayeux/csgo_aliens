using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ProjectileSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {

        ProjectileJob projectileJob = new ProjectileJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        state.Dependency = projectileJob.ScheduleParallel(state.Dependency);
    }
}

partial struct ProjectileJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;
    public void Execute(Entity entity, in ProjectileData projectileData, ref LocalTransform transform)
    {
        transform.Position += DeltaTime * transform.Forward() * projectileData.speed;
    }
}
