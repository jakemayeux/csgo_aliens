using Unity.Burst;
using Unity.Entities;
using Unity.NetCode;
using Unity.Transforms;

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct GoInGameServerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EntitiesReference>();
        state.RequireForUpdate<NetworkId>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);

        EntitiesReference entitiesReference = SystemAPI.GetSingleton<EntitiesReference>();

        foreach (var (receiveCommand, entity) in SystemAPI.Query<RefRO<ReceiveRpcCommandRequest>>().WithAll<GoInGameRequestRpc>().WithEntityAccess())
        {
            ecb.AddComponent<NetworkStreamInGame>(receiveCommand.ValueRO.SourceConnection);
            UnityEngine.Debug.Log("Client connected");

            Entity playerEntity = ecb.Instantiate(entitiesReference.playerPrefab);
            ecb.SetComponent(playerEntity, LocalTransform.FromPosition(new Unity.Mathematics.float3(
                    UnityEngine.Random.Range(-10, 10), 0, 0
            )));

            NetworkId networkId = SystemAPI.GetComponent<NetworkId>(receiveCommand.ValueRO.SourceConnection);

            ecb.AppendToBuffer(receiveCommand.ValueRO.SourceConnection, new LinkedEntityGroup
            {
                Value = playerEntity
            });

            ecb.AddComponent(playerEntity, new GhostOwner
            {
                NetworkId = networkId.Value
            });

            ecb.DestroyEntity(entity);

        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}
