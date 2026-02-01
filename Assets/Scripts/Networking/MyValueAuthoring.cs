using UnityEngine;
using Unity.Entities;
using Unity.NetCode;

public class MyValueAuthoring : MonoBehaviour
{

    class Baker : Baker<MyValueAuthoring>
    {

        public override void Bake(MyValueAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new MyValue
            {

            });
        }

    }

}


public struct MyValue : IComponentData
{
    [GhostField] public int value;
}