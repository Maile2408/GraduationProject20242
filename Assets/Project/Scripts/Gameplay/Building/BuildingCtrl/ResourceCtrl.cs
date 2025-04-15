using UnityEngine;

public class ResourceCtrl : BuildingCtrl
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
        ResetBuildingTaskType();
    }

    protected virtual void ResetBuildingTaskType()
    {
        this.buildingTaskType = BuildingTaskType.resource;
    }
}
