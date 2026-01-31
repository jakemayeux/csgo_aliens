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
        foreach(var (mover, transform, entity) in SystemAPI.Query<RefRW<ZombieMover>, RefRW<LocalTransform>>().WithEntityAccess())
        {
            Entity zombie = entity;
            //Calculate next position 
            float3 destination = mover.ValueRO.destination;
            float3 currentPosition = transform.ValueRO.Position;
            
            float3 trajectory = math.normalize(destination - currentPosition);
            
            float3 nextPosition = trajectory* mover.ValueRO.speed * SystemAPI.Time.DeltaTime;
            
            transform.ValueRW.Position = nextPosition;
        }
    }
}
