using UnityEngine;

public class ConstructionCtrl : SaiBehaviour
{
    [Header("Construction Info")]
    [SerializeField] protected BuildingInfo buildingInfo;
    public AbstractConstruction abstractConstruction;
    public LimitRadius limitRadius;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadAbstractConstruction();
        this.LoadLimitRadius();
    }

    protected virtual void LoadAbstractConstruction()
    {
        if (this.abstractConstruction != null) return;
        this.abstractConstruction = GetComponent<AbstractConstruction>();
    }

    protected virtual void LoadLimitRadius()
    {
        if (this.limitRadius != null) return;
        this.limitRadius = GetComponentInChildren<LimitRadius>();
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
}
