using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    [Header("Building Information")]
    [SerializeField] BuildingData buildingData;

    public BuildingData Data => buildingData;

    public int WorkerCapacity => buildingData.workerCapacity;
    public float StorageCapacity => buildingData.storageCapacity;
    public float ProductionTime => buildingData.productionTime;
    public float GoldPerCycle => buildingData.goldPerCycle;

    public System.Collections.Generic.List<ResourceIO> Produces => buildingData.produces;
    public System.Collections.Generic.List<ResourceIO> Requires => buildingData.requires;
    public System.Collections.Generic.List<ResourceAmount> BuildCost => buildingData.cost;
}
