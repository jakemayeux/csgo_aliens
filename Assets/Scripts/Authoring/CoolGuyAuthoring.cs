using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class CoolGuyAuthoring : MonoBehaviour
{

    class Baker : Baker<CoolGuyAuthoring>
    {

        public override void Bake(CoolGuyAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new CoolGuy
            {

            });
        }


    }

}

public struct CoolGuy : IComponentData
{

}