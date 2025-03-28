using UnityEngine;

public class FarmTask : BuildingTask
{
    [Header("Farm")]
    [SerializeField] protected float waterCost = 3;
    [SerializeField] protected float grainReceive = 24;
}