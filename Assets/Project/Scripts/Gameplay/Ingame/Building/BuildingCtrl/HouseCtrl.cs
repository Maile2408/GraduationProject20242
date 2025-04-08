using UnityEngine;

public class HouseCtrl : BuildingCtrl
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        ResetBuildingTaskType();
    }

    protected virtual void ResetBuildingTaskType()
    {
        this.buildingTaskType = BuildingTaskType.home;
    }
}
