using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingResConsumer : ResGenerator
{
    [SerializeField] private BuildingInfo buildingInfo;

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
        this.LoadResRequire();
    }

    protected override void Creating()
    {
        if (!this.IsRequireEnough())
        {
            this.canCreate = false;
            return;
        }

        this.canCreate = true;

        this.createTimer += Time.fixedDeltaTime;
        if (this.createTimer < this.createDelay) return;
        this.createTimer = 0f;

        this.ConsumeResources();
    }

    protected virtual void LoadResRequire()
    {
        this.resRequire.Clear();

        foreach (var resIO in buildingInfo.CurrentLevelData.requires)
        {
            if (System.Enum.TryParse(resIO.resource, true, out ResourceName resourceName))
            {
                int amount = Mathf.RoundToInt(resIO.amountPerCycle ?? 0);
                Resource res = new Resource
                {
                    name = resourceName,
                    number = amount
                };
                this.resRequire.Add(res);
            }
            else
            {
                Debug.LogWarning($"Unknown resource name: {resIO.resource}");
            }
        }
    }

    protected override bool IsRequireEnough()
    {
        foreach (var res in this.resRequire)
        {
            ResHolder holder = this.GetResource(res.name);
            if (holder == null || holder.Current() < res.number)
                return false;
        }

        return true;
    }

    protected virtual void ConsumeResources()
    {
        foreach (var res in this.resRequire)
        {
            ResHolder holder = this.GetResource(res.name);
            if (holder != null)
            {
                holder.Deduct(res.number);
            }
        }
    }
}
