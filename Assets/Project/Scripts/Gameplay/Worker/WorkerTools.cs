using System.Collections.Generic;
using UnityEngine;

public class WorkerTools : SaiBehaviour
{
    [Header("Tool References")]
    [SerializeField] UnityEngine.GameObject axe;
    [SerializeField] UnityEngine.GameObject box;
    [SerializeField] UnityEngine.GameObject hammer;
    [SerializeField] UnityEngine.GameObject pickaxe;
    [SerializeField] UnityEngine.GameObject saw;
    [SerializeField] UnityEngine.GameObject can;
    UnityEngine.GameObject currentTool;
    WorkerCtrl workerCtrl;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        if (workerCtrl == null) workerCtrl = GetComponent<WorkerCtrl>();
    }

    protected override void FixedUpdate()
    {
        this.UpdateToolByState();
    }

    protected void UpdateToolByState()
    {
        if (currentTool != null) currentTool.SetActive(false);
        currentTool = null;

        if (workerCtrl.workerMovement == null) return;

        if (workerCtrl.workerMovement.isWorking)
        {
            switch (workerCtrl.workerMovement.workingType)
            {
                case WorkingType.chopping: currentTool = axe; break;
                case WorkingType.building: currentTool = hammer; break;
                case WorkingType.sawing: currentTool = saw; break;
                case WorkingType.mining: currentTool = pickaxe; break;
                case WorkingType.watering: currentTool = can; break;
                default: currentTool = null; break;
            }
        }
        else if (workerCtrl.workerMovement.isMoving && 
                 workerCtrl.workerMovement.movingType == MovingType.carrying)
        {
            currentTool = box;
        }

        if (currentTool != null)
            currentTool.SetActive(true);
    }
}
