using Unity.Collections;
using Unity.Entities;
using UnityEngine;

partial struct HumanSpawningSystem : ISystem
{
    public int numberOfHumans;

    public void OnCreate(ref SystemState state)
    {
        //Spawn them at selected locations
    }

    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();

        if (gameManager.ValueRO.GameManagerStates.Equals(GameManagerStates.SpawningInitialHumans))
        {
            
            NativeArray<HumanSpawnPosition> humanSpawnPositions = SystemAPI.GetSingletonBuffer<HumanSpawnPosition>().ToNativeArray(Allocator.Temp);
            foreach(HumanSpawnPosition spawnPos in humanSpawnPositions)
            {
                
            }
        }
    }
}
