

using Unity.Entities;
using Unity.Mathematics;

public struct HumanMover : IComponentData
{
    public float speed;
    public float3 destination;
}

public struct HumanSpawner : IComponentData
{
    public Rectangle SpawnZone;
    public Entity Prefab;

}

public struct HumanReference
{
    public Entity Entity;
    public float3 Position;
}




