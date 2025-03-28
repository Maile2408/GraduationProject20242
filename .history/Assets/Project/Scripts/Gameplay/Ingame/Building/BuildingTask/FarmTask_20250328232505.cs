using System.Collections.Generic;
using UnityEngine;

public class FarmTask : BuildingTask
{
    [Header("Farm")]
    [SerializeField] protected float waterCost = 3;
    [SerializeField] protected float grainReceive = 24;
    [SerializeField] protected List<GrainLevel> grains;
    [SerializeField] private List<Transform> workingPoints;

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
}