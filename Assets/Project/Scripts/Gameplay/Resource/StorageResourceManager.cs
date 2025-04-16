using System.Collections.Generic;
using UnityEngine;

public class StorageResourceManager : SaiBehaviour
{
    public static StorageResourceManager Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only 1 StorageResourceManager allowed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public Dictionary<ResourceName, float> GetTotalResources()
    {
        Dictionary<ResourceName, float> totals = new();

        foreach (var building in BuildingManager.Instance.BuildingCtrls())
        {
            if (building.warehouse is not WarehouseWH wh) continue;

            foreach (var holder in wh.GetStockedResources())
            {
                ResourceName res = holder.Name();
                float amount = holder.Current();

                if (!totals.ContainsKey(res)) totals[res] = 0;
                totals[res] += amount;
            }
        }

        return totals;
    }

    public float GetTotalResourceAmount(ResourceName resourceName)
    {
        float total = 0f;

        foreach (var building in BuildingManager.Instance.BuildingCtrls())
        {
            if (building.warehouse is not WarehouseWH wh) continue;

            foreach (var holder in wh.GetStockedResources())
            {
                if (holder.Name() == resourceName)
                    total += holder.Current();
            }
        }

        return total;
    }

    public bool HasEnoughResources(List<Resource> requiredResources)
    {
        var totals = GetTotalResources();

        foreach (var req in requiredResources)
        {
            if (!totals.TryGetValue(req.name, out float available) || available < req.number)
                return false;
        }

        return true;
    }

    public (float used, float capacity) GetTotalUsedAndCapacity()
    {
        float totalUsed = 0f;
        float totalCapacity = 0f;

        foreach (var building in BuildingManager.Instance.BuildingCtrls())
        {
            if (building.warehouse is not WarehouseWH wh) continue;

            totalCapacity += wh.GetStorageCapacity();

            foreach (var holder in wh.GetStockedResources())
            {
                totalUsed += holder.Current();
            }
        }

        return (totalUsed, totalCapacity);
    }

    public void ReportStorageAchievements()
    {
        var totals = GetTotalResources();
        float total = 0f;

        foreach (var pair in totals)
        {
            float amount = pair.Value;
            total += amount;

            switch (pair.Key)
            {
                case ResourceName.logwood: AchievementReporter.Logwood(amount); break;
                case ResourceName.plank: AchievementReporter.Plank(amount); break;
                case ResourceName.water: AchievementReporter.Water(amount); break;
                case ResourceName.stone: AchievementReporter.Stone(amount); break;
                case ResourceName.grain: AchievementReporter.Grain(amount); break;
                case ResourceName.flour: AchievementReporter.Flour(amount); break;
            }
        }

        AchievementReporter.StoreResource(total);
    }
}
