using System.Collections.Generic;
using UnityEngine;

public class FarmTask : BuildingTask
{
    [Header("Farm")]
    [SerializeField] protected float waterCost = 3;
    [SerializeField] protected float grainReceive = 24;
    [SerializeField] protected List<GrainLevel> grains;
}