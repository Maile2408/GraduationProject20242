using UnityEngine;
using UnityEngine.AI;

public class WorkerCtrl : SaiBehaviour, IPoolable, ISaveable<WorkerSaveData>
{
    public string workerType;
    public WorkerBuildings workerBuildings;
    public WorkerMovement workerMovement;
    public WorkerTasks workerTasks;
    public Animator animator;
    public Transform workerModel;
    public WorkerTools tools;
    public NavMeshAgent navMeshAgent;
    public ResCarrier resCarrier;
    public WorkerGroundAlign workerGroundAlign;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerBuildings();
        this.LoadWorkerMovement();
        this.LoadAnimator();
        this.LoadWorkerTasks();
        this.LoadAgent();
        this.LoadResCarrier();
        this.LoadWokerTools();
        this.LoadWorkerGroundAlign();
    }

    protected virtual void LoadWorkerGroundAlign()
    {
        if (this.workerGroundAlign != null) return;
        this.workerGroundAlign = GetComponent<WorkerGroundAlign>();
        //Debug.Log(transform.name + ": LoadWorkerGroundAlign", gameObject);
    }

    protected virtual void LoadWorkerTasks()
    {
        if (this.workerTasks != null) return;
        this.workerTasks = GetComponent<WorkerTasks>();
        //Debug.Log(transform.name + ": LoadWorkerTasks", gameObject);
    }

    protected virtual void LoadWokerTools()
    {
        if (this.tools != null) return;
        this.tools = GetComponent<WorkerTools>();
        //Debug.Log(transform.name + ": LoadWorkerTools", gameObject);
    }

    protected virtual void LoadAnimator()
    {
        if (this.animator != null) return;
        this.animator = GetComponentInChildren<Animator>();
        this.workerModel = this.animator.transform;
        //Debug.Log(transform.name + ": LoadAnimator", gameObject);
    }

    protected virtual void LoadWorkerBuildings()
    {
        if (this.workerBuildings != null) return;
        this.workerBuildings = GetComponent<WorkerBuildings>();
        //Debug.Log(transform.name + ": LoadWorkerBuildings", gameObject);
    }

    protected virtual void LoadWorkerMovement()
    {
        if (this.workerMovement != null) return;
        this.workerMovement = GetComponent<WorkerMovement>();
        //Debug.Log(transform.name + ": LoadWorkerMovement", gameObject);
    }

    protected virtual void LoadAgent()
    {
        if (this.navMeshAgent != null) return;
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        this.navMeshAgent.speed = 2f;
        //Debug.Log(transform.name + ": LoadAgent", gameObject);
    }

    protected virtual void LoadResCarrier()
    {
        if (this.resCarrier != null) return;
        this.resCarrier = GetComponent<ResCarrier>();
        //Debug.Log(transform.name + ": ResCarrier", gameObject);
    }

    public virtual void WorkerReleased()
    {
        this.workerTasks.readyForTask = false;
        this.workerTasks.taskWorking.GoOutBuilding();
        this.workerBuildings.WorkerReleased();
        this.workerTasks.ClearAllTasks();
        this.workerMovement.SetWorkingType(false, WorkingType.none);
    }

    public void OnSpawn() { }

    public void OnDespawn()
    {
        this.workerBuildings.WorkerReleased();
    }

    // ===================== SAVE ======================
    public WorkerSaveData Save()
    {
        return new WorkerSaveData
        {
            id = GetComponent<Identifiable>().ID,
            type = this.workerType,
            position = transform.position,
            rotation = transform.rotation,
        };
    }

    // ===================== LOAD ======================
    public void LoadFromSave(WorkerSaveData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;

        if (TimeManager.Instance.IsDay) this.workerTasks.GoWork();
        else this.workerTasks.GoHome();
    }
}
