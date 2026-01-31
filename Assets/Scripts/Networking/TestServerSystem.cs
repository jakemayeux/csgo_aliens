using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct TestServerSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);


        foreach (var (simpleRpc, receiveCommand, entity) in SystemAPI.Query<RefRW<SimpleRpc>, RefRW<ReceiveRpcCommandRequest>>().WithEntityAccess())
        {
            ecb.DestroyEntity(entity);
            Debug.Log("Receieved RPC... " + simpleRpc.ValueRO.value);
        }
    }


}
