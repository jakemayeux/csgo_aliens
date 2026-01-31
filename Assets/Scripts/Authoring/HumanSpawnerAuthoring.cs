using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class HumanSpawnerAuthoring : MonoBehaviour
{
    public float X;
    public float Z;

    public float3 Center;
    public GameObject Prefab;

    public List<Transform> SpawnLocations;

    class Baker : Baker<HumanSpawnerAuthoring>
    {
        public override void Bake(HumanSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new HumanSpawner
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),

                SpawnZone = new Rectangle
                {
                    X = authoring.X,
                    Z = authoring.Z,
                    Center = authoring.Center
                }
            });

            AddBuffer<HumanSpawnPosition>(entity);

            foreach(Transform spawnLocation in authoring.SpawnLocations)
            {
                AppendToBuffer(entity, new HumanSpawnPosition
                {
                    SpawnLocation = spawnLocation.position
                });
            } 
            
        }
    }
}


[InternalBufferCapacity(7)]
public struct HumanSpawnPosition: IBufferElementData
{
    public float3 SpawnLocation;
}
