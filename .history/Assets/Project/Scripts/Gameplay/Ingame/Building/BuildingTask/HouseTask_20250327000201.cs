using System.Collections;
using UnityEngine;

public class HouseTask : BuildingTask
{
    [Header("House")]
    [SerializeField] protected float waterCollectTime = 3f;
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
        if (this.wellTarget == null) return;
        if (this.buildingCtrl.warehouse.IsFull()) return;
        workerCtrl.workerMovement.SetTarget(null);

        workerCtrl.workerTasks.TaskAdd(TaskType.goToHome);
        workerCtrl.workerTasks.TaskAdd(TaskType.takeWater);
        workerCtrl.workerTasks.TaskAdd(TaskType.gotoWell);
        workerCtrl.workerTasks.TaskAdd(TaskType.findWell);
    }

    protected virtual void FindWell(WorkerCtrl workerCtrl)
    {
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
        if (workerCtrl.workerMovement.isWorking) return;
        StartCoroutine(this.CollectingWater(workerCtrl));
    }

    protected virtual IEnumerator CollectingWater(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.isWorking = true;
        workerCtrl.workerMovement.workingType = WorkingType.collecting;
        workerCtrl.tools.UpdateTool(MovingType.walking, WorkingType.collecting);

        yield return new WaitForSeconds(this.waterCollectTime);

        workerCtrl.workerMovement.isWorking = false;
        workerCtrl.tools.ClearTool();

        workerCtrl.resCarrier.Add(new Resource(ResourceName.water, 1));
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void GoToHome(WorkerCtrl workerCtrl)
    {
        WorkerTask taskWorking = workerCtrl.workerTasks.taskWorking;
        taskWorking.GotoBuilding();

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        taskWorking.GoIntoBuilding();
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    public virtual void SetWell(GameObject well)
    {
        this.wellTarget = well;
    }
}
