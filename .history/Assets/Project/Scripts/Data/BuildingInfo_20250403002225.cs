using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewBuilding", menuName = "Building/Building Info")]
public class BuildingInfo : ScriptableObject
{
    public string buildingName;
    public Sprite icon;
    public GameObject prefab;
    [TextArea] public string description;
    public bool isUnlocked = true;

    public BuildingCategory category;

    public List<Resource> cost;
}