using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;


public struct ZombieMover : IComponentData
{
    public float speed;
    public float3 destination;
}
