using System;
using Unity.Entities;
using Random = Unity.Mathematics.Random;


partial struct HumanMoverSystem : ISystem
{
    int amountOfHumans;

    Random random;

    public void OnCreate(ref SystemState state)
    {
        random = new Random((uint)UnityEngine.Random.Range(1, int.MaxValue));

        
        
    }

    public void OnUpdate(ref SystemState state)
    {
        
    }
}
