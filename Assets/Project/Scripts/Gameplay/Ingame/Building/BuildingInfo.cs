using UnityEngine;

public class BuildingInfo : MonoBehaviour
{
    [SerializeField] private Building buildingData;

    public Building BuildingData => buildingData;
    public BuildingType BuildingType { get; private set; }
    public BuildingLevel CurrentLevelData { get; private set; }

    public void SetBuilding(Building data)
    {
        this.buildingData = data;
        InitData();
    }

    private void InitData()
    {
        if (!int.TryParse(buildingData.typeID, out int typeId))
        {
            Debug.LogError("Invalid typeID");
            return;
        }

        BuildingType = BuildingTypeDatabase.Instance.GetBuildingByID(typeId);
        if (BuildingType == null)
        {
            Debug.LogError("BuildingType not found");
            return;
        }

        CurrentLevelData = BuildingType.levels.Find(lv => lv.level == buildingData.level);
        if (CurrentLevelData == null)
        {
            Debug.LogError($"Level {buildingData.level} not found");
        }
    }
}
