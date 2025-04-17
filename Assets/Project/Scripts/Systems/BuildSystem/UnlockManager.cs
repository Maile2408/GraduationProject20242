using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    [Header("Runtime Unlocks")]
    private List<BuildingInfo> allBuildings;
    [SerializeField] List<int> unlockedBuildings = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only one UnlockManager allowed!");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        allBuildings = Resources.LoadAll<BuildingInfo>(PoolPrefabPath.BuildingInfo("")).ToList();
    }

    private void Start()
    {
        UnlockInitialBuildings(CityLevelManager.Instance.GetCurrentLevel());
    }

    public bool IsUnlocked(int buildingID)
    {
        return unlockedBuildings.Contains(buildingID);
    }

    public void UnlockBuilding(int buildingID)
    {
        if (!IsUnlocked(buildingID))
        {
            unlockedBuildings.Add(buildingID);
            Debug.Log($"[Unlock] Building Unlocked: {buildingID}");

            // Achievement
            AchievementReporter.UnlockBuilding();
        }
    }

    public void UnlockInitialBuildings(int currentLevel)
    {
        foreach (var b in allBuildings)
        {
            if (b.unlockCityLevel <= currentLevel)
                UnlockBuilding(b.buildingID);
        }
    }

    public List<BuildingInfo> GetUnlockedBuildings()
    {
        return allBuildings.Where(b => IsUnlocked(b.buildingID)).ToList();
    }

    public List<BuildingInfo> GetUnlockedBuildingsByCategory(BuildingCategory category)
    {
        return allBuildings
            .Where(b => b.category == category && IsUnlocked(b.buildingID))
            .ToList();
    }
}
