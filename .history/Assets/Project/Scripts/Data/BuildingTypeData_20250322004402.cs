using System.Collections.Generic;

[System.Serializable]
public class BuildingTypeData
{
    public List<BuildingType> buildingTypes;
}

[System.Serializable]
public class BuildingType
{
    public int id;
    public string name;
    public string category;
    public string description;
    public bool unlocked;
    public List<BuildingLevel> levels;
}

[System.Serializable]
public class BuildingLevel
{
    public int level;
    public int? workerCapacity;
    public int? storageCapacity;
    public float? goldPerCycle;
    public float? productionTime;
    public Dictionary<string, float> cost;
    public List<ResourceIO> produces;
    public List<ResourceIO> requires;
}

[System.Serializable]
public class ResourceIO
{
    public string resource;
    public float? amountPerCycle;
    public int? maxCapacity;
}
