using System.Collections;
using UnityEngine;

public class MineTask : BuildingTask
{
    [Header("Mine")]
    [SerializeField] protected Transform workingPoint;
    [SerializeField] protected float stoneReceive = 1;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkingPoint();
    }

    protected virtual void LoadWorkingPoint()
    {
        if (this.workingPoint != null) return;
        this.workingPoint = transform.Find("WorkingPoint");
        //Debug.Log(transform.name + " LoadObjects", gameObject);
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.makingResource:
                this.MakingResource(workerCtrl);
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
        if (!this.buildingCtrl.warehouse.IsFull())
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
            workerCtrl.workerTasks.TaskAdd(TaskType.makingResource);
            workerCtrl.workerTasks.TaskAdd(TaskType.gotoWorkingPoint);
        }
    }

    protected virtual void MakingResource(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.workerMovement.isWorking) return;
        StartCoroutine(Mining(workerCtrl));
    }

    IEnumerator Mining(WorkerCtrl workerCtrl)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.mining);
        yield return new WaitForSeconds(this.workingSpeed);

        this.buildingCtrl.warehouse.AddResource(ResourceName.stone, this.stoneReceive);

        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.mining);
        workerCtrl.workerTasks.TaskCurrentDone();
    }

    protected virtual void GotoWorkingPoint(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inBuilding) workerTasks.taskWorking.GoOutBuilding();

        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null) workerCtrl.workerMovement.SetTarget(this.workingPoint);

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        //workerCtrl.workerMovement.SetTarget(null);
        workerCtrl.workerTasks.TaskCurrentDone();
    }

}