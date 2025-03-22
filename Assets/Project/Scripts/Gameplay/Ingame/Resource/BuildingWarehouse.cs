using System.Collections.Generic;
using UnityEngine;

public class BuildingWarehouse : Warehouse
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

        this.SetLimit();
    }

    public override bool IsFull()
    {
        float? storageCapacity = buildingInfo.CurrentLevelData.storageCapacity;

        if (storageCapacity != null)
        {
            float total = 0f;

            foreach (ResHolder resHolder in this.resHolders)
            {
                total += resHolder.Current();
            }

            return total >= storageCapacity.Value;
        }
        else
        {
            foreach (ResHolder resHolder in this.resHolders)
            {
                if (!resHolder.IsMax()) return false;
            }

            return true;
        }
    }

    protected virtual void SetLimit()
    {
        foreach (var resIO in buildingInfo.CurrentLevelData.produces)
        {
            if (System.Enum.TryParse(resIO.resource, out ResourceName resourceName))
            {
                ResHolder holder = this.GetResource(resourceName);
                float max = resIO.maxCapacity ?? holder.Max();
                holder.SetLimit(max);
            }
        }

        foreach (var resIO in buildingInfo.CurrentLevelData.requires)
        {
            if (System.Enum.TryParse(resIO.resource, out ResourceName resourceName))
            {
                ResHolder holder = this.GetResource(resourceName);
                float max = resIO.maxCapacity ?? holder.Max();
                holder.SetLimit(max);
            }
        }

    }

    public override ResHolder ResNeed2Move()
    {
        foreach (var produce in buildingInfo.CurrentLevelData.produces)
        {
            if (System.Enum.TryParse(produce.resource, out ResourceName resName))
            {
                ResHolder holder = this.GetResource(resName);
                if (holder != null && holder.Current() > 0)
                {
                    return holder;
                }
            }
        }

        return null;
    }


    public override List<Resource> NeedResoures()
    {
        List<Resource> resources = new List<Resource>();

        foreach (var required in buildingInfo.CurrentLevelData.requires)
        {
            if (System.Enum.TryParse(required.resource, out ResourceName resName))
            {
                ResHolder holder = this.GetResource(resName);
                if (holder == null) continue;

                float current = holder.Current();
                float max = holder.Max();
                float needed = max - current;

                if (needed > 0)
                {
                    resources.Add(new Resource
                    {
                        name = resName,
                        number = needed
                    });
                }
            }
        }

        return resources;
    }
}
