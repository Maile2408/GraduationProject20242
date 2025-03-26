using System.Collections.Generic;
using UnityEngine;

public class HouseTask : BuildingTask
{
    [Header("House")]
    [SerializeField] protected GameObject wellTarget;

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.findWell:
                this.FindWell(workerCtrl);
                break;
            case TaskType.gotoWell:
                this.GoToWell(workerCtrl);
                break;
            case TaskType.takeWater:
                this.TakeWater(workerCtrl);
                break;
            case TaskType.goToHome:
                this.GoToHome(workerCtrl);
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
        if (this.wellTarget == null) this.FindNearestWell();
        if (this.wellTarget == null) return;
        if (this.buildingCtrl.warehouse.IsFull()) return;
        if (workerCtrl.resCarrier.Resources().Count > 0) return;

        workerCtrl.workerMovement.SetTarget(null);

        workerCtrl.workerTasks.TaskAdd(TaskType.goToHome);
        workerCtrl.workerTasks.TaskAdd(TaskType.takeWater);
        workerCtrl.workerTasks.TaskAdd(TaskType.gotoWell);
        workerCtrl.workerTasks.TaskAdd(TaskType.findWell);
    }

    protected virtual void FindWell(WorkerCtrl workerCtrl)
    {
        if (this.wellTarget == null) this.FindNearestWell();
        if (this.wellTarget == null) return;

        workerCtrl.workerTasks.taskTarget = this.wellTarget.transform;
        workerCtrl.workerMovement.SetTarget(this.wellTarget.transform);
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void GoToWell(WorkerCtrl workerCtrl)
    {
        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void TakeWater(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.resCarrier.Resources().Count >= 1) return;

        workerCtrl.workerMovement.movingType = MovingType.carrying;
        workerCtrl.tools.UpdateTool(MovingType.carrying, null);

        workerCtrl.resCarrier.AddResource(ResourceName.water, 1);
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void GoToHome(WorkerCtrl workerCtrl)
    {
        WorkerTask taskWorking = workerCtrl.workerTasks.taskWorking;
        taskWorking.GotoBuilding();

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        List<Resource> resources = workerCtrl.resCarrier.TakeAll();
        this.buildingCtrl.warehouse.AddByList(resources);

        workerCtrl.workerMovement.movingType = MovingType.walking;
        workerCtrl.tools.ClearTool();

        taskWorking.GoIntoBuilding();
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void FindNearestWell()
    {
        float minDistance = float.MaxValue;
        GameObject closest = null;

        foreach (var b in BuildingManager.instance.BuildingCtrls())
        {
            if (!b.CompareTag("Well")) continue;

            float dist = Vector3.Distance(this.transform.position, b.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = b.gameObject;
            }
        }

        this.wellTarget = closest;
    }

    public virtual void SetWell(GameObject well)
    {
        this.wellTarget = well;
    }
}
