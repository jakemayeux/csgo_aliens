using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Collections;
using Random = Unity.Mathematics.Random;


partial struct HumanMoverSystem : ISystem
{
    int amountOfHumans;

    Random random;

    public void OnCreate(ref SystemState state)
    {
        random = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));
    }

    public void OnUpdate(ref SystemState state)
    {
        HumanMoverJob moverJob = new HumanMoverJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime
        };

        state.Dependency = moverJob.ScheduleParallel(state.Dependency);
    }
}

[BurstCompile]
partial struct HumanMoverJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;

    public void Execute(Entity entity, in HumanMover mover, ref LocalTransform transform)
    {
        float3 destination = mover.destination;
        float3 currentPosition = transform.Position;

        float3 trajectory = math.normalize(destination - currentPosition);

        float3 nextPosition = currentPosition + trajectory * mover.speed * DeltaTime;

        transform.Position = nextPosition;
    }
}
