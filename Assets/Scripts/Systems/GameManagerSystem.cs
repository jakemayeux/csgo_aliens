using Unity.Entities;
using Unity.Collections;
using System.Linq;
partial struct GameManagerSystem : ISystem
{

    
    public void OnCreate(ref SystemState state)
    {
        
    }

    public void OnUpdate(ref SystemState state)
    {
        NativeHashMap<int, int> autoTransitions = new NativeHashMap<int, int>(0, Allocator.Temp);
        autoTransitions[(int)GameManagerStates.SpawningInitialZombies] =  (int)GameManagerStates.SpawningInitialHumans;

        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();

        if (autoTransitions.ContainsKey((int)gameManager.ValueRO.GameManagerStates))
        {
            GameManagerStates newState = (GameManagerStates)autoTransitions[(int)gameManager.ValueRO.GameManagerStates];
            gameManager.ValueRW.GameManagerStates = newState;
        }
    }
}
