using Unity.Entities;
using Unity.Collections;
using System.Diagnostics;
partial struct GameManagerSystem : ISystem
{

    
    public void OnCreate(ref SystemState state)
    {
        UnityEngine.Debug.Log("Are you out there?");
        state.RequireForUpdate<GameManager>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();
       //UnityEngine.Debug.Log($"Current state is {gameManager.ValueRO.GameManagerStates}");
        
        NativeHashMap<int, int> autoTransitions = new NativeHashMap<int, int>(0, Allocator.Temp);
        autoTransitions[(int)GameManagerStates.Starting] =  (int)GameManagerStates.SpawningInitialZombies;

        autoTransitions[(int)GameManagerStates.SpawningInitialZombies] =  (int)GameManagerStates.SpawningInitialHumans;


        if (autoTransitions.ContainsKey((int)gameManager.ValueRO.GameManagerStates))
        {
            GameManagerStates newState = (GameManagerStates)autoTransitions[(int)gameManager.ValueRO.GameManagerStates];
            gameManager.ValueRW.GameManagerStates = newState;
        }
    }
}
