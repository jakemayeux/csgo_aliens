using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEditor.XR;

partial struct ZombieMoverSystem : ISystem
{


    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameManager>();

    }

    public void OnUpdate(ref SystemState state)
    {
        //over a frame it gets commands from other systems from entities
        var ECB = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);


        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();

        //NativeList<float3> humanPositions = new NativeList<float3>(0, Allocator.TempJob);
        NativeList<HumanReference> humans = new NativeList<HumanReference>(0, Allocator.TempJob);

        foreach (var (human, localTransform, entity) in SystemAPI.Query<RefRO<HumanMover>, RefRO<LocalTransform>>().WithAbsent<ZombieTag>().WithEntityAccess())
        {
            HumanReference reference = new HumanReference
            {
                Entity = entity,
                Position = localTransform.ValueRO.Position
            };


            humans.Add(reference);

        }

        ZombieMoverJob moverJob = new ZombieMoverJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            humanReferences = humans,
            ECB = ECB.AsParallelWriter()
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

    [ReadOnly] public NativeList<HumanReference> humanReferences;   

    public bool touchedHuman; 

    public EntityCommandBuffer.ParallelWriter ECB;

    public void Execute(Entity entity, in ZombieMover mover, ref LocalTransform transform)
    {
        HumanReference humanTarget = GetClosestHuman(transform.Position, humanReferences);
        float3 currentPosition = transform.Position;

        float3 trajectory = math.normalize(humanTarget.Position - currentPosition);

        float3 nextPosition = currentPosition + trajectory * mover.speed * DeltaTime;

        transform.Position = nextPosition;

        if(math.distancesq(transform.Position, GetClosestHuman(transform.Position, humanReferences).Position) <= 0.5f)
        {
            UnityEngine.Debug.Log("Zombie touched meeeee!");
            touchedHuman = true;
            ECB.AddComponent<ZombieTag>(entity.Index, humanTarget.Entity); 
        }

    }

    public HumanReference GetClosestHuman(float3 myPosition, NativeList<HumanReference> humanReferences)
    {
        HumanReference closestHuman = humanReferences[0];

        float minDistance = math.INFINITY;

        foreach(HumanReference reference in humanReferences)
        {
            float distance = math.distancesq(myPosition, reference.Position);
            if(distance < minDistance)
            {
                closestHuman = reference;
                minDistance = distance;
            }
        }

        return closestHuman;
    }

}