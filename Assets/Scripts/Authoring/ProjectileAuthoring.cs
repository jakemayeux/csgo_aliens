using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class ProjectileAuthoring : MonoBehaviour
{
    public float ProjectileSpeed = 100f;
    public float StunDuration = 3.0f;
    public float3 Trajectory;

}

class ProjectileAuthoringBaker : Baker<ProjectileAuthoring>
{
    public override void Bake(ProjectileAuthoring authoring)
    {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(entity, new WeaponData
        {
           speed = authoring.ProjectileSpeed,
           stunDuration = authoring.StunDuration
        });

        AddComponent(entity, new ProjectileData
        {
            speed = authoring.ProjectileSpeed,
            stunDuration = authoring.StunDuration,
            
        });
    }
}
