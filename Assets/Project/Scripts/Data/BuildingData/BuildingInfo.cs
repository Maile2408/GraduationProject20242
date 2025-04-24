using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Building/Building Info")]
public class BuildingInfo : ScriptableObject
{
    public int buildingID;
    public string buildingName;
    public Sprite icon;
    public GameObject prefab;
    [TextArea] public string description;
    public BuildingCategory category;

    public float coin;
    public List<Resource> cost;

    public int unlockCityLevel = 1;

    public SpecialBuildingAchievementType specialAchievementType = SpecialBuildingAchievementType.None;
}