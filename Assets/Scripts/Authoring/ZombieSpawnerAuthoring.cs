using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ZombieSpawnerAuthoring: MonoBehaviour 
{

    public float X;
    public float Z;

    public float3 Center;

    public GameObject Prefab;
    class Baker: Baker<ZombieSpawnerAuthoring>
    {
        public override void Bake(ZombieSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ZombieSpawner
            {
                Prefab = GetEntity(authoring.Prefab, TransformUsageFlags.Dynamic),

                SpawnZone = new Rectangle{
                    X = authoring.X,
                    Z = authoring.Z,
                    Center = authoring.Center
                    
                }
            });
        }
    }
}
