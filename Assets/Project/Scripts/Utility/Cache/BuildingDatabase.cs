using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildingDatabase : MonoBehaviour
{
    public static BuildingDatabase Instance;

    private Dictionary<int, BuildingInfo> infoByID = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadAllBuildingInfo();
    }

    private void LoadAllBuildingInfo()
    {
        BuildingInfo[] loaded = Resources.LoadAll<BuildingInfo>("BuildingInfo/");

        int duplicates = 0;
        foreach (var info in loaded)
        {
            if (info == null) continue;

            if (infoByID.ContainsKey(info.buildingID))
            {
                Debug.LogWarning($"[BuildingDatabase] Duplicate buildingID: {info.buildingID} ({info.buildingName})");
                duplicates++;
                continue;
            }

            infoByID[info.buildingID] = info;
        }

        Debug.Log($"[BuildingDatabase] Loaded {infoByID.Count} BuildingInfo(s). Duplicates: {duplicates}");
    }

    public BuildingInfo GetByID(int id)
    {
        if (infoByID.TryGetValue(id, out var info))
            return info;

        Debug.LogError($"[BuildingDatabase] BuildingInfo not found for ID: {id}");
        return null;
    }

    public List<BuildingInfo> GetAll() => infoByID.Values.ToList();

    public bool HasBuilding(int id) => infoByID.ContainsKey(id);
}
