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

    [Header("Core Properties")]
    public int workerCapacity = 0;
    public float storageCapacity = 0;
    public float goldPerCycle = 0;
    public float productionTime = 0;

    public List<ResourceAmount> cost = new();
    public List<ResourceIO> produces = new();
    public List<ResourceIO> requires = new();
}

[System.Serializable]
public class ProductionRecipe
{
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