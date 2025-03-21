using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

public class BuildingTypeDatabase : SaiBehaviour
{
    public static BuildingTypeDatabase Instance { get; private set; }

    private List<BuildingType> allBuildingTypes = new List<BuildingType>();

    public IReadOnlyList<BuildingType> AllBuildingTypes => allBuildingTypes;

    protected override void Awake()
    {
        base.Awake();

        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadBuildingTypes();
    }

    private void LoadBuildingTypes()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Data/BuildingTypes");
        if (jsonFile == null)
        {
            Debug.LogError("ERROR: merged_building_data.json NOT FOUND in Resources/Data/");
            return;
        }

        try
        {
            List<BuildingType> loadedData = JsonConvert.DeserializeObject<List<BuildingType>>(jsonFile.text);
            if (loadedData == null || loadedData.Count == 0)
            {
                Debug.LogError("ERROR: JSON is empty or malformed.");
                return;
            }

            allBuildingTypes = loadedData;
            Debug.Log($"Loaded {allBuildingTypes.Count} building types from merged JSON.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"JSON parsing failed: {e.Message}");
        }
    }

    public BuildingType GetBuildingByID(int id)
    {
        return allBuildingTypes.Find(building => building.id == id);
    }

    public BuildingType GetBuildingByName(string name)
    {
        return allBuildingTypes.Find(building => building.name == name);
    }
}
