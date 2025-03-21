using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResGenerator : ResGenerator
{
    [SerializeField] BuildingInfo buildingInfo;
    protected override void LoadComponents()
    {
        base.LoadComponents();

        buildingInfo = GetComponent<BuildingInfo>();
        if (buildingInfo == null || buildingInfo.CurrentLevelData == null)
        {
            Debug.LogError("BuildingInfo missing or not initialized");
            return;
        }

        this.LoadResCreate();
        this.SetLimit();
    }

    protected virtual void LoadResCreate()
    {
        this.resCreate.Clear();
        foreach (string resName in buildingInfo.CurrentLevelData.produces)
        {
            if (System.Enum.TryParse(resName, out ResourceName resourceName))
            {
                Resource res = new Resource
                {
                    name = resourceName,
                    number = Mathf.RoundToInt(buildingInfo.CurrentLevelData.goldProduction)
                };
                this.resCreate.Add(res);
            }
        }
    }

    protected virtual void SetLimit()
    {
        foreach (string resName in buildingInfo.CurrentLevelData.produces)
        {
            if (System.Enum.TryParse(resName, out ResourceName resourceName))
            {
                ResHolder holder = this.GetResource(resourceName);
                holder.SetLimit((int)(buildingInfo.CurrentLevelData.storageCapacity ?? 0));
            }
        }
    }
}
