using UnityEngine;

public class HouseTask : BuildingTask
{
    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        if (!this.IsTime2Work()) return;

        //string message = workerCtrl.name + " Working at " + transform.name;
        //Debug.Log(message, gameObject);
    }
}
