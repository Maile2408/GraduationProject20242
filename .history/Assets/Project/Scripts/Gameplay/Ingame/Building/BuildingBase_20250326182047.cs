using UnityEngine;

public class BuildingBase : MonoBehaviour
{
    [Header("Building Info")]
    public BuildingData buildingData;
    [Tooltip("Level bắt đầu từ 1")]
    public int currentLevel = 1;

    public BuildingLevelData CurrentLevelData
    {
        get
        {
            if (buildingData == null || buildingData.levels == null || buildingData.levels.Count == 0)
                return null;

            int index = Mathf.Clamp(currentLevel - 1, 0, buildingData.levels.Count - 1);
            return buildingData.levels[index];
        }
    }

    public bool CanUpgrade()
    {
        return currentLevel < buildingData.levels.Count;
    }

    public void Upgrade()
    {
        if (!CanUpgrade()) return;

        currentLevel++;
        Debug.Log($"{buildingData.buildingName} upgraded to level {currentLevel}");
    }
}
