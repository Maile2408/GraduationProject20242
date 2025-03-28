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
        if (!this.IsStoreMax() && this.HasWater())
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
            workerCtrl.workerTasks.TaskAdd(TaskType.makingResource);
            workerCtrl.workerTasks.TaskAdd(TaskType.gotoWorkingPoint);
        }
    }

    protected virtual bool IsStoreMax()
    {
        ResHolder grain = this.buildingCtrl.warehouse.GetResource(ResourceName.grain);
        return grain.IsMax();
    }

    protected virtual bool HasWater()
    {
        ResHolder water = this.buildingCtrl.warehouse.GetResource(ResourceName.water);
        return water.Current() > 0;
    }
}