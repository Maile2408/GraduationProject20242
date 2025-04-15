using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilderTask : BuildingTask
{
    [Header("House Builder")]
    [SerializeField] protected List<BuildingCtrl> warehouses;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.findWarehouseHasRes:
                this.FindWarehouseHasRes(workerCtrl);
                break;
            case TaskType.getResNeed2Move:
                this.GetResNeed2Move(workerCtrl);
                break;
            case TaskType.bringResourceBack:
                this.BringResToConstruction(workerCtrl);
                break;
            case TaskType.buildConstruction:
                this.DoBuild(workerCtrl);
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
        if (workerCtrl.workerTasks.taskConstruction == null)
        {
            AbstractConstruction construction = ConstructionManager.Instance.GetConstruction();
            if (construction == null) return;

            construction.builder = this.buildingCtrl;
            workerCtrl.workerTasks.taskConstruction = construction;
        }

        workerCtrl.workerTasks.TaskAdd(TaskType.findWarehouseHasRes);
        this.FindWarehouse();
    }

    protected virtual void FindWarehouse()
    {
        foreach (BuildingCtrl buildingCtrl in this.nearBuildings)
        {
            if (buildingCtrl.buildingTask is not WarehouseTask) continue;
            if (this.warehouses.Contains(buildingCtrl)) continue;
            this.warehouses.Add(buildingCtrl);
        }
    }

    protected virtual void FindWarehouseHasRes(WorkerCtrl workerCtrl)
    {
        var construction = workerCtrl.workerTasks.taskConstruction;
        if (construction == null) return;

        ResourceName resRequireName = construction.GetResRequireName();
        if (resRequireName == ResourceName.noResource)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            workerCtrl.workerTasks.TaskAdd(TaskType.buildConstruction);
            return;
        }

        foreach (BuildingCtrl warehouse in this.warehouses)
        {
            ResHolder resHolder = warehouse.warehouse.GetResource(resRequireName);
            if (resHolder.Current() < 1) continue;

            workerCtrl.workerTasks.taskBuildingCtrl = warehouse;
            workerCtrl.workerTasks.TaskCurrentDone();
            workerCtrl.workerTasks.TaskAdd(TaskType.getResNeed2Move);
            return;
        }
    }

    protected virtual void GetResNeed2Move(WorkerCtrl workerCtrl)
    {
        var construction = workerCtrl.workerTasks.taskConstruction;
        if (construction == null) return;

        BuildingCtrl warehouseCtrl = workerCtrl.workerTasks.taskBuildingCtrl;
        ResourceName resRequireName = construction.GetResRequireName();
        ResHolder resHolder = warehouseCtrl.warehouse.GetResource(resRequireName);
        if (resHolder.Current() < 1)
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            workerCtrl.workerTasks.TaskAdd(TaskType.findWarehouseHasRes);
            return;
        }

        if (workerCtrl.workerTasks.inBuilding)
            workerCtrl.workerTasks.taskWorking.GoOutBuilding();

        if (workerCtrl.workerMovement.GetTarget() == null)
            workerCtrl.workerMovement.SetTarget(warehouseCtrl.door);

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        workerCtrl.workerTasks.TaskCurrentDone();
        int carryCount = workerCtrl.resCarrier.CarryCount();
        warehouseCtrl.warehouse.RemoveResource(resRequireName, carryCount);
        workerCtrl.resCarrier.AddResource(resRequireName, carryCount);
        workerCtrl.workerTasks.TaskAdd(TaskType.bringResourceBack);
    }

    protected virtual void BringResToConstruction(WorkerCtrl workerCtrl)
    {
        var construction = workerCtrl.workerTasks.taskConstruction;
        if (construction == null) return;

        if (workerCtrl.workerMovement.GetTarget() == null)
            workerCtrl.workerMovement.SetTarget(construction.transform);

        workerCtrl.workerMovement.SetMovingType(MovingType.carrying);
        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        workerCtrl.workerMovement.SetMovingType(MovingType.walking);
        workerCtrl.workerTasks.TaskCurrentDone();

        Resource res = workerCtrl.resCarrier.TakeFirst();
        construction.AddRes(res.name, res.number);

        if (construction.isReadyToBuild)
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.buildConstruction);
        }
        else
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.findWarehouseHasRes);
        }
    }

    protected virtual void DoBuild(WorkerCtrl workerCtrl)
    {
        var construction = workerCtrl.workerTasks.taskConstruction;
        if (construction == null) return;
        if (workerCtrl.workerMovement.isWorking) return;

        StartCoroutine(BuildingRoutine(workerCtrl, construction));
    }

    protected virtual IEnumerator BuildingRoutine(WorkerCtrl workerCtrl, AbstractConstruction construction)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.building);
        yield return new WaitForSeconds(this.workingSpeed);
        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.building);

        if (construction != null && construction.isReadyToBuild)
        {
            construction.Finish();
            workerCtrl.workerTasks.taskConstruction = null;
        }

        workerCtrl.workerTasks.TaskCurrentDone();
        workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
    }
}
