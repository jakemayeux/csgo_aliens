using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class HumanAuthoring : MonoBehaviour
{
    public float Speed;
    public float3 Destination;

    class Baker: Baker<HumanAuthoring>
    {
        public override void Bake(HumanAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HumanMover
            {
                speed = authoring.Speed,
                destination = authoring.Destination
            });
        }
    }
}
