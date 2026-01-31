using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

partial struct HumanSpawningSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {
        //Spawn them at selected locations

    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();


        if (gameManager.ValueRO.GameManagerStates == GameManagerStates.SpawningInitialHumans)
        {

            NativeArray<HumanSpawnPosition> humanSpawnPositions = SystemAPI.GetSingletonBuffer<HumanSpawnPosition>().ToNativeArray(Allocator.Temp);

            HumanSpawner spawner = SystemAPI.GetSingleton<HumanSpawner>();

            foreach (HumanSpawnPosition spawnPos in humanSpawnPositions)
            {
                Entity human = state.EntityManager.Instantiate(spawner.Prefab);
                SystemAPI.GetComponentRW<LocalTransform>(human).ValueRW.Position = spawnPos.SpawnLocation;
            }



        }
    }
}
