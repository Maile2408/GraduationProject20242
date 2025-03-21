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

        this.createDelay = buildingInfo.CurrentLevelData.productionTime ?? 5f;
        this.LoadResCreate();
        this.SetLimit();
    }

    protected override void Creating()
    {
        if (this.IsAllResMax())
        {
            this.canCreate = false;
            return;
        }

        this.canCreate = true;
        base.Creating();
    }

    protected virtual void LoadResCreate()
    {
        this.resCreate.Clear();

        foreach (var resIO in buildingInfo.CurrentLevelData.produces)
        {
            if (System.Enum.TryParse(resIO.resource, out ResourceName resourceName))
            {
                int amount = Mathf.RoundToInt(resIO.amountPerCycle ?? 0);
                Resource res = new Resource
                {
                    name = resourceName,
                    number = amount
                };
                this.resCreate.Add(res);
            }
            else
            {
                Debug.LogWarning($"Unknown resource name: {resIO.resource}");
            }
        }
    }

    protected virtual void SetLimit()
    {
        foreach (var resIO in buildingInfo.CurrentLevelData.produces)
        {
            if (System.Enum.TryParse(resIO.resource, out ResourceName resourceName))
            {
                int max = resIO.maxCapacity ?? 0;
                ResHolder holder = this.GetResource(resourceName);
                holder.SetLimit(max);
            }
        }
    }
}
