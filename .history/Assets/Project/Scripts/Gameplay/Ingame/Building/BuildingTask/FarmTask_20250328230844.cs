using UnityEngine;

public class FarmTask : BuildingTask
{
    [Header("Farm")]
    [SerializeField] protected float logwoodCost = 1;
    [SerializeField] protected float grainReceive = 2;
}