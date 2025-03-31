using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Building/Building Type")]
public class BuildingType : ScriptableObject
{
    [Header("Basic Info")]
    public string id;
    public string buildingName;
    [TextArea] public string description;

    [Header("Category")]
    public BuildingCategory category;

    [Header("Unlock Condition")]
    public bool isUnlocked = true;
    public string unlockCondition;

    [Header("Prefab")]
    public GameObject ghostPrefab;
    public GameObject completedPrefab;

    [Header("UI")]
    public Sprite icon;
    public Sprite previewImage;

    [Header("Build Settings")]
    public int goldCost;
    public List<ResourceAmount> resourceCosts;
    public int workerSlots;

    [Header("Production Info")]
    public List<ResourceIO> requires;
    public List<ResourceIO> produces;
}

[System.Serializable]
public class ResourceAmount
{
    public ResourceName resourceName;
    public int amount;
}

[System.Serializable]
public class ResourceIO
{
    public ResourceName resourceName;
    public int amountPerCycle;
}
