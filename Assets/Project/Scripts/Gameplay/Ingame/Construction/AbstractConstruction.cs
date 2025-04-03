using System.Collections.Generic;
using UnityEngine;

public class AbstractConstruction : SaiBehaviour
{
    [Header("Construction Data")]
    public BuildingInfo info;
    public BuildingCtrl builder;
    public bool isPlaced = false;

    protected Dictionary<ResourceName, float> resourceProgress = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public virtual void Setup(BuildingInfo buildingInfo)
    {
        this.info = buildingInfo;
        this.resourceProgress.Clear();
        this.isPlaced = true;

        foreach (var res in info.cost)
        {
            resourceProgress[res.name] = 0f;
        }

        ConstructionManager.Instance.AddConstruction(this);
    }

    public virtual ResourceName GetResRequireName()
    {
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

        resourceProgress[name] += amount;
        float maxAmount = info.cost.Find(r => r.name == name)?.number ?? 0;
        resourceProgress[name] = Mathf.Min(resourceProgress[name], maxAmount);
    }

    public virtual float Percent()
    {
        float total = 0f;
        float current = 0f;

        foreach (var res in info.cost)
        {
            float required = res.number;
            float progress = resourceProgress.ContainsKey(res.name) ? resourceProgress[res.name] : 0;

            total += required;
            current += Mathf.Min(progress, required);
        }

        return total <= 0 ? 100f : (current / total) * 100f;
    }

    public virtual void Finish()
    {
        if (this.info == null || this.info.prefab == null) return;

        string realPrefabName = this.info.prefab.name.Replace("_Ghost", "");

        GameObject realBuilding = PoolManager.Instance.Spawn(PoolPrefabPath.Building(realPrefabName));
        realBuilding.transform.position = this.transform.position;

        if (realBuilding.TryGetComponent(out AlignWithGround align))
            align.Align();

        if (realBuilding.TryGetComponent(out BuildingCtrl ctrl))
            BuildingManager.Instance.AddBuilding(ctrl);

        ConstructionManager.Instance.RemoveConstruction(this);
        PoolManager.Instance.Despawn(this.gameObject);
    }
}
