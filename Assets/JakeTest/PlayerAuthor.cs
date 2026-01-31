using Unity.Jobs;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

partial struct PlayerSystem : ISystem
{
    public void OnCreate(ref SystemState state) {}

    public void OnUpdate(ref SystemState state) {
        PlayerJob job = new PlayerJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            InputX = Input.GetAxis("Horizontal"),
            InputZ = Input.GetAxis("Vertical"),
            Forward = Camera.main.transform.forward,
            Right = Camera.main.transform.right
        };

        state.Dependency = job.ScheduleParallel(state.Dependency);
    }
}

public class PlayerAuthor : MonoBehaviour 
{
    public GameObject Prefab;
    class Baker: Baker<PlayerAuthor>
    {
        public override void Bake(PlayerAuthor author)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerData
            {
                Prefab = GetEntity(author.Prefab, TransformUsageFlags.Dynamic),
            });
        }
    }
}

public struct PlayerData : IComponentData
{
    public Entity Prefab;
    float3 velocity;
}

partial struct PlayerJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;
    [ReadOnly] public float InputX;
    [ReadOnly] public float InputZ;
    [ReadOnly] public Vector3 Forward;
    [ReadOnly] public Vector3 Right;

    public void Execute(Entity entity, in PlayerData playerData, ref LocalTransform transform)
    {
        Forward.y = 0f;
        Forward.Normalize();
        float3 move_direction = Right * InputX + Forward * InputZ;

        transform.Position += DeltaTime * move_direction * 100;
    }
}