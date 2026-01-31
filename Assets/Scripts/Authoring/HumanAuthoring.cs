using Unity.Entities;
using UnityEngine;

public class HumanAuthoring : MonoBehaviour
{
    public float speed;

    class Baker: Baker<HumanAuthoring>
    {
        public override void Bake(HumanAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HumanMover
            {
                speed = authoring.speed
            });
        }
    }
}
