using System.Collections;
using UnityEngine;

public class MillTask : BuildingTask
{
    [Header("Mill")]
    [SerializeField] protected float grainCost = 3;
    [SerializeField] protected float flourReceive = 1;
    [SerializeField] protected Transform relaxingPoint;
    [SerializeField] protected float relaxingTime = 10;
    [SerializeField] protected Transform fanBlades;
    [SerializeField] protected float fanSpeed = 100f;

    protected bool isProcessing = false;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadRelaxingPoint();
    }

    protected virtual void LoadRelaxingPoint()
    {
        if (this.relaxingPoint != null) return;
        this.relaxingPoint = transform.Find("RelaxingPoint");
        //Debug.Log(transform.name + " LoadRelaxingPoint", gameObject);
    }

    protected override void Update()
    {
        if (isProcessing && fanBlades != null)
        {
            fanBlades.Rotate(Vector3.up, fanSpeed * Time.deltaTime);
        }
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.makingResource:
                this.MakingResource(workerCtrl);
                break;
            case TaskType.goOutToRelax:
                this.GotoRelaxingPoint(workerCtrl);
                break;
            case TaskType.goToWorkStation:
                this.GoToWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTime2Work()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void Planning(WorkerCtrl workerCtrl)
    {
        if (!this.IsStoreMax() && this.HasGrain())
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.goOutToRelax);
            workerCtrl.workerTasks.TaskAdd(TaskType.makingResource);
        }
    }

    protected virtual void MakingResource(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.workerMovement.isWorking) return;
        StartCoroutine(Processing(workerCtrl));
    }

    IEnumerator Processing(WorkerCtrl workerCtrl)
    {
        isProcessing = true;
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.relaxing);
        yield return new WaitForSeconds(this.workingSpeed * 2);

        this.buildingCtrl.warehouse.RemoveResource(ResourceName.grain, this.grainCost);
        this.buildingCtrl.warehouse.AddResource(ResourceName.flour, this.flourReceive);

        isProcessing = false;
        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.relaxing);

        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void GotoRelaxingPoint(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inBuilding) workerTasks.taskWorking.GoOutBuilding();

        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) workerCtrl.workerMovement.SetTarget(this.relaxingPoint);

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        StartCoroutine(Relaxing(workerCtrl));
    }

    IEnumerator Relaxing(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.relaxing);
        yield return new WaitForSeconds(this.relaxingTime);

        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.relaxing);

        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
    }

    protected virtual bool IsStoreMax()
    {
        ResHolder flour = this.buildingCtrl.warehouse.GetResource(ResourceName.flour);
        return flour.IsMax();
    }

    protected virtual bool HasGrain()
    {
        ResHolder grain = this.buildingCtrl.warehouse.GetResource(ResourceName.grain);
        return grain.Current() >= this.grainCost;
    }
}