using Unity.Entities;
using Unity.Mathematics;

public struct WeaponData : IComponentData
{
    public float speed;
    public Entity launchLocation;
    public Entity projectilePrefab;
    public float3 trajectory;
    public float stunDuration;
    public Entity weaponPrefab;
}

public struct ProjectileData: IComponentData
{
    public float speed;
    public float stunDuration;
    public float3 trajectory;
}