using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct ZombieMoverSystem : ISystem
{

    
    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        ZombieMoverJob moverJob = new ZombieMoverJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        state.Dependency = moverJob.ScheduleParallel(state.Dependency);
    }
}

public struct DeathComponent : IComponentData
{

}


[BurstCompile]
[WithAbsent(typeof(DeathComponent))]

partial struct ZombieMoverJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;

    public void Execute(Entity entity, in ZombieMover mover, ref LocalTransform transform)
    {

        float3 destination = mover.destination;
        float3 currentPosition = transform.Position;

        float3 trajectory = math.normalize(destination - currentPosition);

        float3 nextPosition = currentPosition + trajectory * mover.speed * DeltaTime;

        transform.Position = nextPosition;

    }



}