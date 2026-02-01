using Unity.Entities;
using Unity.Mathematics;

public struct WeaponData : IComponentData
{
    public float speed;
    public float3 origin;
    public float3 trajectory;
    public float stunDuration;
}