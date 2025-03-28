using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmTask : BuildingTask
{
    [Header("Farm")]
    [SerializeField] protected float waterCost = 3;
    [SerializeField] protected float grainReceive = 24;
    [SerializeField] protected List<GrainLevel> grains;
    [SerializeField] protected Transform workingPoint;
    [SerializeField] protected Transform grainsRoot;

    protected enum FarmPhase { WaitingForWatering, Growing, WaitingForHarvest }
    protected FarmPhase phase = FarmPhase.WaitingForWatering;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadGrains();
        this.LoadWorkingPoint();
        this.grainsRoot?.gameObject.SetActive(false);
    }

    protected virtual void LoadWorkingPoint()
    {
        if (this.workingPoint != null) return;
        this.workingPoint = transform.Find("WorkingPoint");
        Debug.Log(transform.name + " LoadObjects", gameObject);
    }

    protected virtual void LoadGrains()
    {
        grainsRoot = transform.Find("Grains");

        this.grains.Clear();
        if (this.grains.Count == 0 && this.grainsRoot != null)
        {
            foreach (Transform child in this.grainsRoot)
            {
                GrainLevel grain = child.GetComponent<GrainLevel>();
                if (grain != null) this.grains.Add(grain);
            }
        }
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.watering:
                this.DoWatering(workerCtrl);
                break;
            case TaskType.harvesting:
                this.DoHarvesting(workerCtrl);
                break;
            case TaskType.gotoWorkingPoint:
                this.GotoWorkingPoint(workerCtrl);
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
        if (this.IsStoreMax()) return;

        switch (phase)
        {
            case FarmPhase.WaitingForWatering:
                this.PlanWatering(workerCtrl);
                break;
            case FarmPhase.WaitingForHarvest:
                this.PlanHarvest(workerCtrl);
                break;
        }
    }

    protected virtual void PlanWatering(WorkerCtrl workerCtrl)
    {
        if (!this.HasWater()) return;

        workerCtrl.workerMovement.SetTarget(this.workingPoint);
        workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
        workerCtrl.workerTasks.TaskAdd(TaskType.watering);
        workerCtrl.workerTasks.TaskAdd(TaskType.gotoWorkingPoint);
    }

    protected virtual void PlanHarvest(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.SetTarget(this.workingPoint);
        workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
        workerCtrl.workerTasks.TaskAdd(TaskType.harvesting);
        workerCtrl.workerTasks.TaskAdd(TaskType.gotoWorkingPoint);
    }

    protected virtual void DoWatering(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.workerMovement.isWorking) return;
        StartCoroutine(Watering(workerCtrl));
    }

    IEnumerator Watering(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.watering);
        yield return new WaitForSeconds(this.workingSpeed);

        this.buildingCtrl.warehouse.RemoveResource(ResourceName.water, this.waterCost);

        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.watering);
        workerCtrl.workerTasks.TaskCurrentDone();

        this.grainsRoot.gameObject.SetActive(true);

        foreach (GrainLevel grain in grains)
        {
            grain.ResetGrowth();
            grain.StartGrow();
        }

        phase = FarmPhase.Growing;
    }

    protected virtual void DoHarvesting(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.workerMovement.isWorking) return;
        StartCoroutine(Harvesting(workerCtrl));
    }

    IEnumerator Harvesting(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.harvest);
        yield return new WaitForSeconds(this.workingSpeed);

        this.grainsRoot.gameObject.SetActive(false);

        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.harvest);
        workerCtrl.workerTasks.TaskCurrentDone();

        phase = FarmPhase.WaitingForWatering;

        this.buildingCtrl.warehouse.AddResource(ResourceName.grain, this.grainReceive);
    }

    protected virtual void GotoWorkingPoint(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inBuilding) workerTasks.taskWorking.GoOutBuilding();

        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) return;

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        workerCtrl.workerMovement.SetTarget(null);
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual bool IsStoreMax()
    {
        ResHolder grain = this.buildingCtrl.warehouse.GetResource(ResourceName.grain);
        return grain.IsMax();
    }

    protected virtual bool HasWater()
    {
        ResHolder water = this.buildingCtrl.warehouse.GetResource(ResourceName.water);
        return water.Current() >= this.waterCost;
    }

    protected override void Update()
    {
        if (phase == FarmPhase.Growing)
        {
            bool allReady = this.grains.TrueForAll(g => g.CanHarvest());
            if (allReady)
            {
                phase = FarmPhase.WaitingForHarvest;
            }
        }
    }
}
