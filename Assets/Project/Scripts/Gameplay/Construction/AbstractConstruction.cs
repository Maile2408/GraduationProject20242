using System.Collections.Generic;
using UnityEngine;

public class AbstractConstruction : SaiBehaviour
{
    [Header("Construction Data")]
    public BuildingInfo info;
    public BuildingCtrl builder;
    public bool isPlaced = false;

    [Header("Build Progress")]
    protected Dictionary<ResourceName, float> resourceProgress = new();
    public bool isReadyToBuild = false;
    public bool isBuilding = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public virtual void Setup(BuildingInfo buildingInfo)
    {
        this.info = buildingInfo;
        this.resourceProgress.Clear();
        this.isPlaced = true;
        this.isReadyToBuild = false;
        this.isBuilding = false;

        bool requiresNoResource = true;

        foreach (var res in info.cost)
        {
            float preFilled = Mathf.Max(0, res.number - 1);
            resourceProgress[res.name] = preFilled;

            if (res.number > 0) requiresNoResource = false;
        }

        if (requiresNoResource) this.isReadyToBuild = true;

        ConstructionManager.Instance.AddConstruction(this);
    }

    public virtual ResourceName GetResRequireName()
    {
        if (this.isReadyToBuild) return ResourceName.noResource;

        foreach (var res in info.cost)
        {
            if (!resourceProgress.ContainsKey(res.name)) continue;

            float current = resourceProgress[res.name];
            if (current < res.number)
                return res.name;
        }

        return ResourceName.noResource;
    }

    public virtual void AddRes(ResourceName name, float amount)
    {
        if (!resourceProgress.ContainsKey(name)) return;

        float maxAmount = info.cost.Find(r => r.name == name)?.number ?? 0;
        resourceProgress[name] += amount;
        resourceProgress[name] = Mathf.Min(resourceProgress[name], maxAmount);

        this.CheckIfReadyToBuild();
    }

    protected virtual void CheckIfReadyToBuild()
    {
        foreach (var res in info.cost)
        {
            float required = res.number;
            float progress = resourceProgress.ContainsKey(res.name) ? resourceProgress[res.name] : 0;

            if (progress < required)
                return;
        }

        this.isReadyToBuild = true;
    }

    public virtual void Finish()
    {
        if (this.info == null || this.info.prefab == null) return;

        string realPrefabName = this.info.prefab.name.Replace("_Ghost", "");

        GameObject realBuilding = PoolManager.Instance.Spawn(PoolPrefabPath.Building(realPrefabName));
        realBuilding.transform.position = this.transform.position;
        realBuilding.transform.rotation = this.transform.rotation;

        SaveUtils.AssignID(realBuilding, IDType.Building);

        if (realBuilding.TryGetComponent(out AlignWithGround align))
            align.Align();

        if (realBuilding.TryGetComponent(out BuildingCtrl ctrl))
        {
            BuildingManager.Instance.AddBuilding(ctrl);
        }

        ConstructionManager.Instance.NotifyWorkersWhenFinished(this);
        ConstructionManager.Instance.RemoveConstruction(this);
        PoolManager.Instance.Despawn(this.gameObject);

        CityLevelManager.Instance.AddXP(25);
        GameMessage.Info($"Builder finished: {info.buildingName}! +25 XP");

        //Achievement
        AchievementReporter.FirstBuild();
        AchievementReporter.BuildStructure();
        switch (info.specialAchievementType)
        {
            case SpecialBuildingAchievementType.House:
                AchievementReporter.BuildHouse();
                break;

            case SpecialBuildingAchievementType.BuilderHut:
                AchievementReporter.BuildBuilderHut();
                break;

            case SpecialBuildingAchievementType.Warehouse:
                AchievementReporter.BuildWarehouse();
                break;
        }
    }

    public List<Resource> GetResourceProgress()
    {
        var list = new List<Resource>();
        foreach (var pair in resourceProgress)
        {
            list.Add(new Resource
            {
                name = pair.Key,
                number = pair.Value
            });
        }
        return list;
    }

    public void SetResourceProgress(List<Resource> list)
    {
        resourceProgress.Clear();
        foreach (var res in list)
        {
            resourceProgress[res.name] = res.number;
        }

        CheckIfReadyToBuild(); 
    }

    public virtual void ResetConstruction()
    {
        this.builder = null;
    }
}