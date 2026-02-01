using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;


partial struct ZombieSpawningSystem : ISystem
{

    double nextSpawn;
    Random random;


    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<ZombieSpawner>();
        random = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));
    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();

        double CurrentTime = SystemAPI.Time.ElapsedTime;


        if (nextSpawn < CurrentTime)
        {
            nextSpawn = CurrentTime + 0.4;
        }
        else
        {
            return;
        }

        foreach (var (spawner, entity) in SystemAPI.Query<RefRW<ZombieSpawner>>().WithEntityAccess())
        {

            float3 nextPosition = (random.NextFloat3() * 10f) + spawner.ValueRO.SpawnZone.Center;
            nextPosition.y = 0.0f;

            Entity zombie = state.EntityManager.Instantiate(spawner.ValueRO.Prefab);

            SystemAPI.GetComponentRW<LocalTransform>(zombie).ValueRW.Position = nextPosition;

        }



    }
}
