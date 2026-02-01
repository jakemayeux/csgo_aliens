using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;

[UpdateInGroup(typeof(PredictedSimulationSystemGroup))]
partial struct NetcodePlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerInput, transform, entity) in SystemAPI.Query<RefRO<NetcodePlayerInput>, RefRW<LocalTransform>>()
            .WithAll<Simulate>()
            .WithEntityAccess())
        {

            float3 moveVector = new float3(playerInput.ValueRO.playerInput.x, 0, playerInput.ValueRO.playerInput.y);

            transform.ValueRW.Position += moveVector * SystemAPI.Time.DeltaTime * 10;

        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
