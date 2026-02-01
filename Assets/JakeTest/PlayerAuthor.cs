using Unity.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerSystem : ISystem
{

    public void OnCreate(ref SystemState state)
    {

    }

    public void OnUpdate(ref SystemState state)
    {
        PlayerJob job = new PlayerJob
        {
            zombieLookup = SystemAPI.GetComponentLookup<ZombieTag>(true),
            DeltaTime = SystemAPI.Time.DeltaTime,
            InputX = Input.GetAxis("Horizontal"),
            InputZ = Input.GetAxis("Vertical"),
            Forward = Camera.main.transform.forward,
            Right = Camera.main.transform.right,
            speed = 50f,
            zombieSpeedMultiplier = 1.5f,

            //Weapons

        };

        if (Input.GetMouseButtonDown(0) == true)
        {
            Entity playerEntity = SystemAPI.GetSingletonEntity<PlayerData>();

            WeaponData weapon = SystemAPI.GetComponent<WeaponData>(playerEntity);
            Entity projectile = state.EntityManager.Instantiate(weapon.projectilePrefab);
            RefRW<LocalTransform> projectTileTransform = SystemAPI.GetComponentRW<LocalTransform>(projectile);
            projectTileTransform.ValueRW.Position = SystemAPI.GetComponent<LocalToWorld>(weapon.launchLocation).Position;
            projectTileTransform.ValueRW.Rotation = Camera.main.transform.rotation;
            // foreach (var (weapon, transform, entity) in SystemAPI.Query<RefRW<WeaponData>,RefRW<LocalTransform>>().WithEntityAccess())
            // {
            //     Entity projectile = state.EntityManager.Instantiate(weapon.ValueRO.projectilePrefab);
            //     float3 startingPosition = SystemAPI.GetComponentRW<LocalTransform>(projectile).ValueRW.Position = SystemAPI.GetComponent<LocalToWorld>(weapon.ValueRO.launchLocation).Position;
            //     transform.ValueRW.Position = startingPosition;
            //     weapon.ValueRW.trajectory = transform.ValueRO.Forward();

            // }
        }




        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

public class PlayerAuthor : MonoBehaviour
{
    public GameObject Prefab;

    public float Speed = 100f;

    [Header("Weapon References")]
    public Transform WeaponLocation;
    public GameObject WeaponPrefab;
    public GameObject ProjectilePrefab;


    class Baker : Baker<PlayerAuthor>
    {
        public override void Bake(PlayerAuthor author)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerData
            {
                Prefab = GetEntity(author.Prefab, TransformUsageFlags.Dynamic),
            });

            AddComponent(entity, new HumanTag { });

            AddComponent(entity, new WeaponData
            {
                projectilePrefab = GetEntity(author.ProjectilePrefab, TransformUsageFlags.Dynamic),
                launchLocation = GetEntity(author.WeaponLocation, TransformUsageFlags.Dynamic),
                weaponPrefab = GetEntity(author.WeaponPrefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct PlayerData : IComponentData
{
    public Entity Prefab;
    float3 velocity;
}

public struct HumanTag : IComponentData
{

}



partial struct PlayerJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;
    [ReadOnly] public float InputX;
    [ReadOnly] public float InputZ;
    [ReadOnly] public Vector3 Forward;
    [ReadOnly] public Vector3 Right;

    [ReadOnly] public ComponentLookup<ZombieTag> zombieLookup;

    [ReadOnly] public bool WeaponInput;
    [ReadOnly] public WeaponData weaponData;


    public float speed;
    public float zombieSpeedMultiplier;

    public void Execute(Entity entity, in PlayerData playerData, ref LocalTransform transform)
    {
        if (zombieLookup.HasComponent(entity))
        {
            speed *= zombieSpeedMultiplier;
        }
        Forward.y = 0f;
        Forward.Normalize();
        float3 move_direction = Right * InputX + Forward * InputZ;

        transform.Position += DeltaTime * move_direction * speed;

        if (WeaponInput == true)
        {
            //spawn weapon projectile

        }
    }
}

