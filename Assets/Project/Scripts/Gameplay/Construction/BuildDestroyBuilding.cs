using UnityEngine;

public class BuildDestroyBuilding : BuildDestroyable
{
    public BuildingCtrl buildingCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadBuildingCtrl();
    }

    protected virtual void LoadBuildingCtrl()
    {
        if (buildingCtrl != null) return;
        buildingCtrl = GetComponent<BuildingCtrl>();
    }

    public override void Destroy()
    {
        if (!isDestructible) return;

        buildingCtrl.workers.ReleaseWorkers();
        BuildingManager.Instance.RemoveBuilding(buildingCtrl);
        base.Destroy();
    }
}
