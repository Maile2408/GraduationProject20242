using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : SaiBehaviour
{
    public static BuildingManager Instance;
    [SerializeField] protected List<BuildingCtrl> buildingCtrls;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null) Destroy(gameObject);
        Instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadBuildingCtrls();
    }

    protected virtual void LoadBuildingCtrls()
    {
        if (this.buildingCtrls.Count > 0) return;
        foreach (Transform child in transform)
        {
            BuildingCtrl ctrl = child.GetComponent<BuildingCtrl>();
            if (ctrl == null) continue;
            this.buildingCtrls.Add(ctrl);
        }

        //Debug.Log(transform.name + "LoadBuildingCtrls", gameObject);
    }

    public virtual BuildingCtrl FindBuilding(BuildingTaskType buildingTaskType)
    {
        BuildingCtrl candidateWithZeroWorker = null;

        for (int i = 0; i < this.buildingCtrls.Count; i++)
        {
            BuildingCtrl buildingCtrl = this.buildingCtrls[i];
            if (buildingCtrl.buildingTaskType != buildingTaskType) continue;
            if (!buildingCtrl.workers.IsNeedWorker()) continue;

            if (buildingCtrl.workers.WorkerCount() == 0)
            {
                return buildingCtrl;
            }

            if (candidateWithZeroWorker == null)
            {
                candidateWithZeroWorker = buildingCtrl;
            }
        }

        return candidateWithZeroWorker;
    }

    public virtual List<BuildingCtrl> BuildingCtrls()
    {
        return this.buildingCtrls;
    }

    public virtual void AddBuilding(BuildingCtrl buildingCtrl)
    {
        this.buildingCtrls.Add(buildingCtrl);
        buildingCtrl.transform.parent = transform;
        this.NearBuildingRecheck();
    }

    public virtual void RemoveBuilding(BuildingCtrl buildingCtrl)
    {
        this.buildingCtrls.Remove(buildingCtrl);
    }

    protected virtual void NearBuildingRecheck()
    {
        foreach (BuildingCtrl buildingCtrl in this.buildingCtrls)
        {
            buildingCtrl.buildingTask.FindNearBuildings();
        }
    }
}
