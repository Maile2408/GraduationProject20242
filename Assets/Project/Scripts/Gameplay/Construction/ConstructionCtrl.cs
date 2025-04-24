using UnityEngine;

public class ConstructionCtrl : SaiBehaviour, IPoolable, ISaveable<ConstructionSaveData>
{
    [Header("Construction Info")]
    public string constructionType;
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

    // ===================== SAVE ======================
    public ConstructionSaveData Save()
    {
        return new ConstructionSaveData
        {
            id = GetComponent<Identifiable>().ID,
            type = this.constructionType,
            buildingInfoID = this.buildingInfo.buildingID,
            position = transform.position,
            rotation = transform.rotation,
            localScale = transform.localScale,
            resourceProgress = this.abstractConstruction.GetResourceProgress()
        };
    }

    // ===================== LOAD ======================
    public void LoadFromSave(ConstructionSaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
        transform.localScale = data.localScale;

        BuildingInfo info = BuildingDatabase.Instance.GetByID(data.buildingInfoID);
        if (info == null)
        {
            Debug.LogError($"[ConstructionCtrl] BuildingInfo ID {data.buildingInfoID} not found.");
            return;
        }

        Setup(info);
        this.abstractConstruction.SetResourceProgress(data.resourceProgress);
    }
}