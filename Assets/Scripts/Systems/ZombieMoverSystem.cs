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
        state.RequireForUpdate<GameManager>();

    }

    public void OnUpdate(ref SystemState state)
    {
        if (false)
        {
            foreach (var (mover, transform, entity) in SystemAPI.Query<RefRW<ZombieMover>, RefRW<LocalTransform>>().WithEntityAccess())
            {

                //Calculate next position 
                float3 destination = mover.ValueRO.destination;
                float3 currentPosition = transform.ValueRO.Position;

                float3 trajectory = math.normalize(destination - currentPosition);

                float3 nextPosition = currentPosition + trajectory * mover.ValueRO.speed * SystemAPI.Time.DeltaTime;

                transform.ValueRW.Position = nextPosition;

            }
        }

        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();

        NativeList<float3> humanPositions = new NativeList<float3>(0, Allocator.TempJob);

        foreach (var (human, localTransform, entity) in SystemAPI.Query<RefRO<HumanMover>, RefRO<LocalTransform>>().WithEntityAccess())
        {
            humanPositions.Add(localTransform.ValueRO.Position);

        }

        ZombieMoverJob moverJob = new ZombieMoverJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            humanLocations = humanPositions
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

    [ReadOnly] public NativeList<float3> humanLocations;



    public void Execute(Entity entity, in ZombieMover mover, ref LocalTransform transform)
    {
        float3 destination = humanLocations[0];
        float3 currentPosition = transform.Position;

        float3 trajectory = math.normalize(destination - currentPosition);

        float3 nextPosition = currentPosition + trajectory * mover.speed * DeltaTime;

        transform.Position = nextPosition;

    }



}