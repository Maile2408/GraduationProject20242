using System.Collections.Generic;
using UnityEngine;

public class WorkerTask : SaiBehaviour
{
    public WorkerCtrl workerCtrl;
    [SerializeField] protected float buildingDistance = 0;
    [SerializeField] protected float buildDisLimit = 0.7f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (this.GetBuilding()) this.GettingReadyForWork();
        else this.FindBuilding();

        if (workerCtrl.workerTasks.readyForTask) this.Working();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        //this.GoOutBuilding();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrl();
    }

    protected virtual void LoadWorkerCtrl()
    {
        if (this.workerCtrl != null) return;
        this.workerCtrl = transform.parent.parent.GetComponent<WorkerCtrl>();
        //Debug.Log(transform.name + ": LoadWorkerCtrl", gameObject);
    }

    protected virtual void FindBuilding()
    {
        BuildingCtrl buildingCtrl = BuildingManager.Instance.FindBuilding(this.GetBuildingTaskType());
        if (buildingCtrl == null) return;

        buildingCtrl.workers.AddWorker(this.workerCtrl);
        this.AssignBuilding(buildingCtrl);
    }

    public virtual void GotoBuilding()
    {
        BuildingCtrl buildingCtrl = this.GetBuilding();
        this.workerCtrl.workerMovement.SetTarget(buildingCtrl.door);
    }

    public virtual bool IsAtBuilding()
    {
        return this.BuildingDistance() < this.buildDisLimit;
    }

    protected virtual float BuildingDistance()
    {
        BuildingCtrl buildingCtrl = this.GetBuilding();
        this.buildingDistance = Vector3.Distance(transform.position, buildingCtrl.door.transform.position);
        return this.buildingDistance;
    }

    protected virtual void GettingReadyForWork()
    {
        if (this.workerCtrl.workerTasks.readyForTask) return;

        if (!this.workerCtrl.workerMovement.IsClose2Target())
        {
            this.GotoBuilding();
            return;
        }

        this.workerCtrl.workerTasks.readyForTask = true;
        this.GoIntoBuilding();
    }

    public virtual void GoIntoBuilding()
    {
        if (this.workerCtrl.workerTasks.inBuilding) return;

        this.workerCtrl.workerMovement.SetTarget(null);
        this.workerCtrl.workerTasks.inBuilding = true;
        this.workerCtrl.workerModel.gameObject.SetActive(false);
    }

    public virtual void GoOutBuilding()
    {
        if (!this.workerCtrl.workerTasks.inBuilding) return;

        this.workerCtrl.workerTasks.inBuilding = false;
        this.workerCtrl.workerModel.gameObject.SetActive(true);
    }

    protected virtual void Working()
    {
        this.GetBuilding().buildingTask.DoingTask(this.workerCtrl);
    }

    protected virtual BuildingCtrl GetBuilding()
    {
        //For overide
        return null;
    }

    protected virtual void AssignBuilding(BuildingCtrl buildingCtrl)
    {
        //For overide
    }

    protected virtual BuildingTaskType GetBuildingTaskType()
    {
        return BuildingTaskType.workStation;
    }
}
