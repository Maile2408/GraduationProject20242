using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnlockManager : MonoBehaviour
{
    public static UnlockManager Instance;

    [Header("Runtime Unlocks")]
    [SerializeField] private List<int> unlockedBuildings = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UnlockInitialBuildings(CityLevelManager.Instance.Level);
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
            Debug.Log($"[UnlockManager] Unlocked building ID: {buildingID}");
        }
    }

    public void UnlockInitialBuildings(int currentLevel)
    {
        foreach (var b in BuildingDatabase.Instance.GetAll())
        {
            if (b.unlockCityLevel <= currentLevel)
                UnlockBuilding(b.buildingID);
        }
    }

    public List<BuildingInfo> GetUnlockedBuildings()
    {
        return BuildingDatabase.Instance.GetAll()
            .Where(b => IsUnlocked(b.buildingID))
            .ToList();
    }

    public List<BuildingInfo> GetUnlockedBuildingsByCategory(BuildingCategory category)
    {
        return BuildingDatabase.Instance.GetAll()
            .Where(b => b.category == category && IsUnlocked(b.buildingID))
            .ToList();
    }

    public int GetTotalUnlockedBuildingCount() => unlockedBuildings.Count;
}
