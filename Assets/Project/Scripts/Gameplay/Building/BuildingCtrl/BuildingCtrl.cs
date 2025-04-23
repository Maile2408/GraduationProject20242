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
        Debug.Log(transform.name + " LoadWorkers", gameObject);
    }

    protected virtual void LoadDoor()
    {
        if (this.door != null) return;
        this.door = transform.Find("Door");
        Debug.Log(transform.name + " LoadDoor", gameObject);
    }

    protected virtual void LoadWarehouse()
    {
        if (this.warehouse != null) return;
        this.warehouse = GetComponent<Warehouse>();
        Debug.Log(transform.name + " LoadWarehouse", gameObject);
    }

    protected virtual void LoadBuldingTask()
    {
        if (this.buildingTask != null) return;
        this.buildingTask = GetComponent<BuildingTask>();
        Debug.Log(transform.name + ": LoadBuldingTask", gameObject);
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        this.warehouse.ResetResources();
    }

    // ===================== SAVE ======================
    public BuildingSaveData Save()
    {
        return new BuildingSaveData
        {
            //id = GetComponent<Identifiable>().id,
            type = this.buildingType,
            buildingInfoID = this.buildingInfo.buildingID,
            position = transform.position,
            rotation = transform.rotation,
            //inventory = this.warehouse.,
            //taxReady = this.buildingTask.taxReady,
            //taxTimer = this.buildingTask.taxTimer
        };
    }

    // ===================== LOAD ======================
    public void LoadFromSave(BuildingSaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;

        /*if (this.buildingInfo == null || this.buildingInfo.buildingID != data.buildingInfoID)
        {
            // You can optionally reload buildingInfo from ID if needed
            Debug.LogWarning("BuildingInfo mismatch or null: " + name);
        }*/

        //this.warehouse.SetResources(data.inventory);
        //this.buildingTask.taxReady = data.taxReady;
        //this.buildingTask.taxTimer = data.taxTimer;

        // Optionally reset UI, task state, animation...
    }
}
