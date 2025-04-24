using System.Collections.Generic;
using UnityEngine;

public class WarehouseTask : BuildingTask
{
    [Header("Warehouse")]
    [SerializeField] protected int takeProductMax = 7;
    [SerializeField] protected float takeProductDelay = 7f;

    [SerializeField] protected int bringMaterialMax = 2;
    [SerializeField] protected float bringMaterialDelay = 7f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.findBuildingHasProduct:
                this.FindBuildingHasProduct(workerCtrl);
                break;
            case TaskType.gotoGetProduct:
                this.GotoGetProduct(workerCtrl);
                break;
            case TaskType.takingProductBack:
                this.BringResourceBack(workerCtrl);
                break;
            case TaskType.findBuildingNeedMaterial:
                this.FindBuildingNeedMaterial(workerCtrl);
                break;
            case TaskType.bringMatetiral2Building:
                this.BringMatetiral2Building(workerCtrl);
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
        workerCtrl.workerTasks.TaskAdd(TaskType.findBuildingNeedMaterial);
        workerCtrl.workerTasks.TaskAdd(TaskType.findBuildingHasProduct);

        workerCtrl.workerTasks.bringMaterialCount = bringMaterialMax;
        workerCtrl.workerTasks.takeProductCount = takeProductMax;
    }

    protected virtual void FindBuildingHasProduct(WorkerCtrl workerCtrl)
    {
        WorkerTasks tasks = workerCtrl.workerTasks;
        tasks.takeProductTimer += Time.fixedDeltaTime;
        if (tasks.takeProductTimer > takeProductDelay)
        {
            tasks.takeProductCount--;
            tasks.takeProductTimer = 0;
        }

        if (tasks.takeProductCount < 0)
        {
            tasks.TaskCurrentDone();
            return;
        }

        BuildingCtrl buildingCtrl = this.FindBuildingHasProductOld(workerCtrl);
        if (buildingCtrl != null)
        {
            tasks.TaskAdd(TaskType.gotoGetProduct);
            tasks.takeProductTimer = 0;
            tasks.takeProductCount--;
        }
    }

    protected virtual void FindBuildingNeedMaterial(WorkerCtrl workerCtrl)
    {
        WorkerTasks tasks = workerCtrl.workerTasks;
        tasks.bringMaterialTimer += Time.fixedDeltaTime;
        if (tasks.bringMaterialTimer > bringMaterialDelay)
        {
            tasks.bringMaterialCount--;
            tasks.bringMaterialTimer = 0;
        }

        if (tasks.bringMaterialCount < 0)
        {
            tasks.TaskCurrentDone();
            return;
        }

        List<Resource> resources;
        ResHolder resHolder;
        int carryCount = workerCtrl.resCarrier.CarryCount();

        foreach (BuildingCtrl buildingCtrl in this.nearBuildings)
        {
            bool isWorkstationOrHome =
                buildingCtrl.buildingTaskType == BuildingTaskType.workStation ||
                buildingCtrl.buildingTaskType == BuildingTaskType.home;

            if (!isWorkstationOrHome) continue;
            resources = buildingCtrl.warehouse.NeedResoures();
            foreach (Resource resource in resources)
            {
                resHolder = this.buildingCtrl.warehouse.GetResource(resource.name);
                if (resHolder.Current() < carryCount) continue;

                this.buildingCtrl.warehouse.RemoveResource(resource.name, carryCount);
                workerCtrl.resCarrier.AddResource(resource.name, carryCount);

                tasks.taskBuildingCtrl = buildingCtrl;
                tasks.TaskAdd(TaskType.bringMatetiral2Building);

                tasks.bringMaterialCount--;
                tasks.bringMaterialTimer = 0;
                return;
            }
        }
    }

    protected virtual void GotoGetProduct(WorkerCtrl workerCtrl)
    {
        WorkerTasks tasks = workerCtrl.workerTasks;
        if (tasks.inBuilding) tasks.taskWorking.GoOutBuilding();

        BuildingCtrl taskBuildingCtrl = tasks.taskBuildingCtrl;
        ResHolder resHolder = taskBuildingCtrl.warehouse.ResNeed2Move();
        if (resHolder == null)
        {
            this.DoneGetResNeed2Move(workerCtrl);
            return;
        }

        if (workerCtrl.workerMovement.GetTarget() == null)
            workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        float count = workerCtrl.resCarrier.CarryCount();
        resHolder.Deduct(count);
        workerCtrl.resCarrier.AddResource(resHolder.Name(), count);
        this.DoneGetResNeed2Move(workerCtrl);

        tasks.taskBuildingCtrl = this.buildingCtrl;
        tasks.TaskAdd(TaskType.takingProductBack);
    }

    protected virtual void DoneGetResNeed2Move(WorkerCtrl workerCtrl)
    {
        WorkerTasks tasks = workerCtrl.workerTasks;
        tasks.TaskCurrentDone();
        tasks.taskBuildingCtrl = null;
    }

    protected virtual BuildingCtrl FindBuildingHasProductOld(WorkerCtrl workerCtrl)
    {
        int tryCount = 999;
        do
        {
            tryCount--;
            this.lastBuildingWorked++;
            if (lastBuildingWorked >= this.nearBuildings.Count)
            {
                this.lastBuildingWorked = 0;
                break;
            }

            BuildingCtrl nextBuilding = this.nearBuildings[this.lastBuildingWorked];

            bool isWorkstationOrResource =
                nextBuilding.buildingTaskType == BuildingTaskType.workStation ||
                nextBuilding.buildingTaskType == BuildingTaskType.resource;

            if (!isWorkstationOrResource) continue;

            ResHolder resHolder = nextBuilding.warehouse.ResNeed2Move();
            if (resHolder == null) continue;

            if (this.buildingCtrl.warehouse is WarehouseWH centralWarehouse && centralWarehouse.IsFull())
            {
                continue;
            }

            workerCtrl.workerTasks.taskBuildingCtrl = nextBuilding;
            return nextBuilding;

        } while (tryCount > 0);

        return null;
    }

    protected virtual void BringResourceBack(WorkerCtrl workerCtrl)
    {
        WorkerTasks tasks = workerCtrl.workerTasks;
        BuildingCtrl taskBuildingCtrl = tasks.taskBuildingCtrl;
        if (taskBuildingCtrl.warehouse is WarehouseWH wh && wh.IsFull())
        {
            //Debug.Log("Warehouse full, canceling resources.");
            tasks.taskBuildingCtrl = null;
            tasks.TaskCurrentDone();
            return;
        }

        workerCtrl.workerMovement.SetMovingType(MovingType.carrying);
        if (tasks.inBuilding) tasks.taskWorking.GoOutBuilding();

        if (workerCtrl.workerMovement.GetTarget() == null)
            workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        tasks.taskBuildingCtrl = null;
        tasks.TaskCurrentDone();

        workerCtrl.workerMovement.SetMovingType(MovingType.walking);
        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.warehouse.AddResource(res.name, res.number);

        tasks.TaskAdd(TaskType.goToWorkStation);
    }

    protected virtual void BringMatetiral2Building(WorkerCtrl workerCtrl)
    {
        WorkerTasks tasks = workerCtrl.workerTasks;
        workerCtrl.workerMovement.SetMovingType(MovingType.carrying);
        if (tasks.inBuilding) tasks.taskWorking.GoOutBuilding();

        BuildingCtrl taskBuildingCtrl = tasks.taskBuildingCtrl;

        if (workerCtrl.workerMovement.GetTarget() == null)
            workerCtrl.workerMovement.SetTarget(taskBuildingCtrl.door);

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        workerCtrl.workerMovement.SetMovingType(MovingType.walking);
        Resource res = workerCtrl.resCarrier.TakeFirst();
        taskBuildingCtrl.warehouse.AddResource(res.name, res.number);

        tasks.taskBuildingCtrl = null;
        tasks.TaskCurrentDone();

        tasks.TaskAdd(TaskType.goToWorkStation);
    }
}
