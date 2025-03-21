using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json; 

public class BuildingTypeDatabase : SaiBehaviour
{
    public static BuildingTypeDatabase Instance;
    private List<BuildingType> allBuildingTypes = new List<BuildingType>();

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null)
        {
            Instance = this;
            LoadBuildingTypes();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadBuildingTypes()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/BuildingTypes");

        if (jsonFile == null)
        {
            Debug.LogError("ERROR: BuildingTypes.json NOT FOUND in Resources/Data/");
            return;
        }

        //Debug.Log("BuildingTypes.json found, loading...");
        //Debug.Log("JSON Content: " + jsonFile.text); 

        try
        {
            buildingTypeData buildingTypeData = JsonConvert.DeserializeObject<buildingTypeData>(jsonFile.text);

            if (buildingTypeData == null || buildingTypeData.buildingTypes == null || buildingTypeData.buildingTypes.Count == 0)
            {
                Debug.LogError("ERROR: BuildingData.json is EMPTY or INVALID!");
                return;
            }

            allBuildingTypes = buildingTypeData.buildingTypes;
            Debug.Log($"Loaded {allBuildingTypes.Count} buildings from JSON.");
        }
        catch (System.Exception e)
        {
            Debug.LogError("ERROR: JSON Parsing Failed: " + e.Message);
        }
    }

    public BuildingType GetBuildingByID(int id)
    {
        return allBuildingTypes.Find(building => building.id == id);
    }

    public List<BuildingType> GetAllBuildings()
    {
        return allBuildingTypes;
    }
}
