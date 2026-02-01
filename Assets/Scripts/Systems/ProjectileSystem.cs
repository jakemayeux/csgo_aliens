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

        NativeList<ZombieReference> zombies = new NativeList<ZombieReference>(0, Allocator.TempJob);
        foreach(var(zombie, localTransform, entity) in SystemAPI.Query<RefRO<ZombieTag>, RefRO<LocalTransform>>().WithEntityAccess())
        {
            ZombieReference zombieReference = new ZombieReference
            {
                Entity = entity,
                Position = localTransform.ValueRO.Position
                
            };
            zombies.Add(zombieReference);
        }

        ProjectileJob projectileJob = new ProjectileJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            zombieReferences = zombies
        };



        state.Dependency = projectileJob.ScheduleParallel(state.Dependency);

        
    }
}

partial struct ProjectileJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;
    [ReadOnly] public NativeList<ZombieReference> zombieReferences;

    
    public void Execute(Entity entity, in ProjectileData projectileData, ref LocalTransform transform)
    {
        transform.Position += DeltaTime * transform.Forward() * projectileData.speed;

        foreach(ZombieReference zombieReference in zombieReferences)
        {
            float distance = math.distancesq(transform.Position, zombieReference.Position);
            if(distance <= 0.5f && distance > 0f)
            {
                UnityEngine.Debug.Log("Hit Zombie");
            }
        }
    }
}
