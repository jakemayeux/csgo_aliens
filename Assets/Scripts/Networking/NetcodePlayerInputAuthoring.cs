using Unity.NetCode;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class NetcodePlayerInputAuthoring : MonoBehaviour
{

    class Baker : Baker<NetcodePlayerInputAuthoring>
    {
        public override void Bake(NetcodePlayerInputAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new NetcodePlayerInput());
        }

    }

}

public struct NetcodePlayerInput : IInputComponentData
{
    public float2 playerInput;
}