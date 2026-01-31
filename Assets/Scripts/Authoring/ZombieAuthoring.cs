using UnityEngine;
using Unity.Entities;

public class ZombieAuthoring : MonoBehaviour
{
    public float speed;
    
    class Baker: Baker<ZombieAuthoring>
    {
        public override void Bake(ZombieAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ZombieMover
            {
                speed = authoring.speed

            });

            AddComponent(entity, new ZombieTag
            {
                
            });
        }
    }   
}
