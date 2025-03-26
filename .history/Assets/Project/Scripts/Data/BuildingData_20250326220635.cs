using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Building")]
public class BuildingData : ScriptableObject
{
    [Header("Basic information")]
    public int id;
    public string buildingName;
    public string category;
    [TextArea] public string description;
    public Sprite icon;
    public GameObject prefab;

    [Header("Build Information")]
    public List<ResourceAmount> cost = new();                 

    [Header("Core Gameplay")]
    public int workerCapacity = 0;
    public float storageCapacity = 0;
    public float productionTime = 0;
    public float goldPerCycle = 0;

    [Header("Production")]
    public List<ResourceIO> produces = new();   
    public List<ResourceIO> requires = new();   
}

[System.Serializable]
public class ResourceAmount
{
    public ResourceName resource;
    public float amount;
}

[System.Serializable]
public class ResourceIO
{
    public ResourceName resource;
    public float amountPerCycle;
    public float maxCapacity;
}