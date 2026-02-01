using System.ComponentModel;
using Unity.Entities;
using UnityEngine;

public class ConfigAuthoring : MonoBehaviour
{
    class Baker : Baker<ConfigAuthoring>
    {
        public override void Bake(ConfigAuthoring authoring)
        {
            AddTransformUsageFlags(TransformUsageFlags.ManualOverride);
            Entity entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new GameManager
            {
                
            });
        }
    }
}

public struct GameManager : IComponentData
{
    public GameManagerStates GameManagerStates;
}

public enum GameManagerStates
{
    Starting = 0,
    Playing = 10,
    Paused = 20,
    SpawningInitialHumans = 30,
    SpawningInitialZombies = 40
}
