using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;
using Unity.Transforms;
using UnityEngine;

[BurstCompile]
[UpdateInGroup(typeof(GhostInputSystemGroup))]
partial struct NetcodePlayerInputSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<NetworkStreamInGame>();
        state.RequireForUpdate<NetcodePlayerInput>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (playerInput, entity) in SystemAPI.Query<RefRW<NetcodePlayerInput>>()
            .WithAll<GhostOwnerIsLocal>()
            .WithEntityAccess())
        {

            float2 inputVector = new float2();
            if (Input.GetKey(KeyCode.W))
            {
                inputVector.y = 1;
            }

            if (Input.GetKey(KeyCode.S))
            {
                inputVector.y = -1;
            }

            if (Input.GetKey(KeyCode.A))
            {
                inputVector.x = 1;
            }

            if (Input.GetKey(KeyCode.D))
            {
                inputVector.x = -1;
            }

            playerInput.ValueRW.playerInput = inputVector;
        }
    }
}
