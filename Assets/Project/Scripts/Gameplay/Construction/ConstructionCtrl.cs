using UnityEngine;

public class ConstructionCtrl : SaiBehaviour, IPoolable
{
    [Header("Construction Info")]
    [SerializeField] protected BuildingInfo buildingInfo;
    public AbstractConstruction abstractConstruction;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAbstractConstruction();
    }

    protected virtual void LoadAbstractConstruction()
    {
        if (this.abstractConstruction != null) return;
        this.abstractConstruction = GetComponent<AbstractConstruction>();
    }

    public virtual void Setup(BuildingInfo info)
    {
        this.buildingInfo = info;

        if (this.abstractConstruction != null)
        {
            this.abstractConstruction.Setup(info);
        }
    }

    public virtual BuildingInfo GetBuildingInfo()
    {
        return this.buildingInfo;
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        this.abstractConstruction.ResetConstruction();
    }
}