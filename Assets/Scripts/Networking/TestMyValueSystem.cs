
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct TestMyValueSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (myValue, entity) in SystemAPI.Query<RefRO<MyValue>>()
            .WithEntityAccess())
        {

            Debug.Log("My value: " + myValue.ValueRO.value + "   " + entity);


        }
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {
        
    }
}

[WorldSystemFilter(WorldSystemFilterFlags.ServerSimulation)]
partial struct TestMyValueServerSystem : ISystem
{

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (myValue, entity) in SystemAPI.Query<RefRW<MyValue>>()
            .WithEntityAccess())
        {

            Debug.Log("My value: " + myValue.ValueRO.value + "   " + entity);

            if (Input.GetKeyDown(KeyCode.Y))
            {
                myValue.ValueRW.value = UnityEngine.Random.Range(-100, 100);
                Debug.Log(myValue.ValueRW.value);
            }

        }
    }


}