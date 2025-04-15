using UnityEngine;

public class DecoractionCtrl : BuildingCtrl
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        ResetBuildingTaskType();
    }

    protected virtual void ResetBuildingTaskType()
    {
        this.buildingTaskType = BuildingTaskType.none;
    }
}
