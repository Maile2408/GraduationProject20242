using System.Linq;
using UnityEngine;

public class BuildingCtrl : SaiBehaviour, IPoolable, ISaveable<BuildingSaveData>
{
    [Header("Building")]
    public string buildingType;
    public BuildingTaskType buildingTaskType = BuildingTaskType.workStation;
    public Transform door;
    public Workers workers;
    public Warehouse warehouse;
    public BuildingTask buildingTask;
    public BuildingInfo buildingInfo;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkers();
        this.LoadDoor();
        this.LoadWarehouse();
        this.LoadBuldingTask();
    }

    protected virtual void LoadWorkers()
    {
        if (this.workers != null) return;
        this.workers = GetComponent<Workers>();
        //Debug.Log(transform.name + " LoadWorkers", gameObject);
    }

    protected virtual void LoadDoor()
    {
        if (this.door != null) return;
        this.door = transform.Find("Door");
        //Debug.Log(transform.name + " LoadDoor", gameObject);
    }

    protected virtual void LoadWarehouse()
    {
        if (this.warehouse != null) return;
        this.warehouse = GetComponent<Warehouse>();
        //Debug.Log(transform.name + " LoadWarehouse", gameObject);
    }

    protected virtual void LoadBuldingTask()
    {
        if (this.buildingTask != null) return;
        this.buildingTask = GetComponent<BuildingTask>();
        //Debug.Log(transform.name + ": LoadBuldingTask", gameObject);
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        this.warehouse.ResetResources();
    }

    // ===================== SAVE ======================
    public BuildingSaveData Save()
    {
        var data = new BuildingSaveData
        {
            id = GetComponent<Identifiable>().ID,
            type = this.buildingType,
            position = transform.position,
            rotation = transform.rotation,
            inventory = this.warehouse.GetStockedResources()
                .Select(r => new Resource { name = r.Name(), number = r.Current() }).ToList(),
            taxReady = false,
            taxTimer = 0f
        };

        if (TryGetComponent<TaxBuildingCtrl>(out var tax))
        {
            data.taxReady = tax.IsReadyToCollect();
            data.taxTimer = tax.GetCurrentTimer();
        }

        return data;
    }

    // ===================== LOAD ======================
    public void LoadFromSave(BuildingSaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;

        this.warehouse.ResetResources();
        this.warehouse.AddByList(data.inventory);

        if (TryGetComponent<TaxBuildingCtrl>(out var tax))
        {
            tax.RestoreState(data.taxReady, data.taxTimer);
        }
    }

}
