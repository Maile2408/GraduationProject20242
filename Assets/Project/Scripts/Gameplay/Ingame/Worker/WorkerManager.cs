using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : SaiBehaviour
{
    public static WorkerManager Instance;

    [SerializeField] protected List<WorkerCtrl> workerCtrls = new();

    protected override void Awake()
    {
        base.Awake();
        if (WorkerManager.Instance != null) Debug.LogError("Only 1 WorkerManager allow");
        WorkerManager.Instance = this;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrls();
    }

    protected virtual void LoadWorkerCtrls()
    {
        if (this.workerCtrls.Count > 0) return;

        foreach (Transform child in transform)
        {
            WorkerCtrl ctrl = child.GetComponent<WorkerCtrl>();
            if (ctrl == null) continue;
            this.workerCtrls.Add(ctrl);
        }

        Debug.Log(transform.name + ": LoadWorkerCtrls", gameObject);
    }

    public virtual List<WorkerCtrl> WorkerCtrls()
    {
        return this.workerCtrls;
    }

    public virtual void AddWorker(WorkerCtrl workerCtrl)
    {
        if (this.workerCtrls.Contains(workerCtrl)) return;

        this.workerCtrls.Add(workerCtrl);
        workerCtrl.transform.parent = this.transform;
    }

    public virtual void RemoveWorker(WorkerCtrl workerCtrl)
    {
        this.workerCtrls.Remove(workerCtrl);
    }
}
