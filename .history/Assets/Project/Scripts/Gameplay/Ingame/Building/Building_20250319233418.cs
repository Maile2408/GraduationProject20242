using System.Collections.Generic;
using UnityEngine;

public class Building : SaiBehaviour
{
    public int ID { get; private set; }
    public string Name { get; private set; }
    public string Category { get; private set; }
    public int Level { get; private set; }
    public List<ResHolder> ResHolders { get; private set; } = new List<ResHolder>();

    public Building(Building buildingData)
    {
        this.ID = buildingData.id;
        this.Name = buildingData.name;
        this.Category = buildingData.category;
        this.Level = buildingData.levels[0].level;

        InitializeResHolders(buildingData);
    }

    private void InitializeResHolders(Building buildingData)
    {
        if (buildingData.levels == null || buildingData.levels.Count == 0) return;

        Level currentLevel = buildingData.levels[0];

        if (currentLevel.produces != null)
        {
            foreach (string produce in currentLevel.produces)
            {
                ResourceName resource = ResNameParser.FromString(produce);
                ResHolders.Add(new ResHolder(resource, currentLevel.storageCapacity ?? Mathf.Infinity));
            }
        }

        if (currentLevel.requires != null)
        {
            foreach (string require in currentLevel.requires)
            {
                ResourceName resource = ResNameParser.FromString(require);
                ResHolders.Add(new ResHolder(resource, currentLevel.storageCapacity ?? Mathf.Infinity));
            }
        }
    }

    public ResHolder GetResHolder(ResourceName resourceName)
    {
        return ResHolders.Find(r => r.ResourceType == resourceName);
    }
}
