using Unity.Collections;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEditor.XR;

partial struct ZombieMoverSystem : ISystem
{
    int parity;
    public NativeParallelMultiHashMap<int, float3> spatialMap1;
    public NativeParallelMultiHashMap<int, float3> spatialMap2;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<GameManager>();

        parity = 0;
        spatialMap1 = new NativeParallelMultiHashMap<int, float3>(65536, Allocator.Persistent);
        spatialMap2 = new NativeParallelMultiHashMap<int, float3>(65536, Allocator.Persistent);
    }

    public void OnUpdate(ref SystemState state)
    {
        //over a frame it gets commands from other systems from entities
        var ECB = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);


        RefRW<GameManager> gameManager = SystemAPI.GetSingletonRW<GameManager>();

        //NativeList<float3> humanPositions = new NativeList<float3>(0, Allocator.TempJob);
        NativeList<HumanReference> humans = new NativeList<HumanReference>(0, Allocator.TempJob);

        foreach (var (human, localTransform, entity) in SystemAPI.Query<RefRO<HumanMover>, RefRO<LocalTransform>>().WithAbsent<ZombieTag>().WithEntityAccess())
        {
            HumanReference reference = new HumanReference
            {
                Entity = entity,
                Position = localTransform.ValueRO.Position
            };

            humans.Add(reference);

        }

        parity += 1;
        parity = parity % 2;

        NativeParallelMultiHashMap<int, float3> spatialMapOld;
        NativeParallelMultiHashMap<int, float3> spatialMapNew;

        if (parity == 0) {
            spatialMapOld = spatialMap1;
            spatialMapNew = spatialMap2;
        } else {
            spatialMapOld = spatialMap2;
            spatialMapNew = spatialMap1;
        }

        spatialMapNew.Clear();

        ZombieMoverJob moverJob = new ZombieMoverJob
        {
            DeltaTime = SystemAPI.Time.DeltaTime,
            humanReferences = humans,
            target = Camera.main.transform.position,
            spatialMapRead = spatialMapOld,
            spatialMapWrite = spatialMapNew.AsParallelWriter(),
            ECB = ECB.AsParallelWriter()
        };
        state.Dependency = moverJob.ScheduleParallel(state.Dependency);
    }
}

public struct DeathComponent : IComponentData
{

}


[BurstCompile]
[WithAbsent(typeof(DeathComponent))]

partial struct ZombieMoverJob : IJobEntity
{
    [ReadOnly] public float DeltaTime;
 
    [ReadOnly] public NativeList<HumanReference> humanReferences;   
    [ReadOnly] public float3 target;
    [ReadOnly]  public NativeParallelMultiHashMap<int, float3> spatialMapRead;
    [WriteOnly] public NativeParallelMultiHashMap<int, float3>.ParallelWriter spatialMapWrite;

    public bool touchedHuman; 

    public EntityCommandBuffer.ParallelWriter ECB;

    public void Execute(Entity entity, in ZombieMover mover, ref LocalTransform transform)
    {
        //HumanReference humanTarget = GetClosestHuman(transform.Position, humanReferences);
        float3 destination = new float3(target.x, 0f, target.z);
        float3 currentPosition = transform.Position;

        float3 trajectory = math.normalize(destination - currentPosition);

        int2 cell = (int2)math.floor(transform.Position.xy);
        int key = cell.x + cell.y * 128;

        NativeParallelMultiHashMapIterator<int> it;
        float3 pos;
        float count = 1f;
        for (int x = cell.x - 1; x <= cell.x + 1; x++) {
            for (int y = cell.y - 1; y <= cell.y + 1; y++) {
                key = x + y * 128;
                if (spatialMapRead.TryGetFirstValue(key, out pos, out it))
                {
                    do
                    {
                        float3 direction = currentPosition - pos;
                        if (math.lengthsq(direction) > 0.1 && math.lengthsq(direction) <= 1) {
                            trajectory += math.normalize(direction);
                            count ++;
                        }
                    }
                    while (spatialMapRead.TryGetNextValue(out pos, ref it));
                }
            }
        }

        trajectory /= count;

        float3 nextPosition = currentPosition + trajectory * mover.speed * DeltaTime * 3f;
        transform.Position = nextPosition;
        spatialMapWrite.Add(key, nextPosition);

        //if(math.distancesq(transform.Position, GetClosestHuman(transform.Position, humanReferences).Position) <= 0.5f)
        //{
        //    UnityEngine.Debug.Log("Zombie touched meeeee!");
        //    touchedHuman = true;
        //    ECB.AddComponent<ZombieTag>(entity.Index, humanTarget.Entity); 
        //}

    }

    public HumanReference GetClosestHuman(float3 myPosition, NativeList<HumanReference> humanReferences)
    {
        HumanReference closestHuman = humanReferences[0];

        float minDistance = math.INFINITY;

        foreach(HumanReference reference in humanReferences)
        {
            float distance = math.distancesq(myPosition, reference.Position);
            if(distance < minDistance)
            {
                closestHuman = reference;
                minDistance = distance;
            }
        }

        return closestHuman;
    }

}