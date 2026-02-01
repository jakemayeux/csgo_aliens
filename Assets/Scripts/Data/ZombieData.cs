using Unity.Entities;
using Unity.Mathematics;


public struct ZombieMover : IComponentData
{
    public float speed;
    public float3 destination;
}

public struct ZombieSpawner : IComponentData
{
    public Rectangle SpawnZone;
    public Entity Prefab;
}
public struct ZombieTag: IComponentData
{
    //Empty, just to find zombies
}

public struct ZombieReference: IComponentData
{
    public Entity Entity;
    public float3 Position;
}

public struct Stunned: IComponentData
{
    
}

public struct Rectangle
{
    public float X;
    public float Z;

    public float3 Center;
}
