using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json; 

public class BuildingDatabase : SaiBehaviour
{
    public static BuildingDatabase Instance;
    private List<Building> allBuildings = new List<Building>();

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            LoadBuildingData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadBuildingData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/BuildingData");

        if (jsonFile == null)
        {
            Debug.LogError("ERROR: BuildingData.json NOT FOUND in Resources/Data/");
            return;
        }

        Debug.Log("BuildingData.json found, loading...");
        Debug.Log("JSON Content: " + jsonFile.text); 

        try
        {
            BuildingData buildingData = JsonConvert.DeserializeObject<BuildingData>(jsonFile.text);

            if (buildingData == null || buildingData.buildings == null || buildingData.buildings.Count == 0)
            {
                Debug.LogError("ERROR: BuildingData.json is EMPTY or INVALID!");
                return;
            }

            allBuildings = buildingData.buildings;
            Debug.Log($"Loaded {allBuildings.Count} buildings from JSON.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("ERROR: JSON Parsing Failed: " + e.Message);
        }
    }

    public Building GetBuildingByID(int id)
    {
        return allBuildings.Find(building => building.id == id);
    }

    public List<Building> GetAllBuildings()
    {
        return allBuildings;
    }
}
