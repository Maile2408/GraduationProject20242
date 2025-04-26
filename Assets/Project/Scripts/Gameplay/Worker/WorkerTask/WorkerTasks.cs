using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerTasks : SaiBehaviour
{
    public WorkerCtrl workerCtrl;
    public bool inBuilding = false;
    public bool readyForTask = false;
    public TaskWorking taskWorking;
    public TaskGoHome taskGoHome;
    public Transform taskTarget;
    public BuildingCtrl taskBuildingCtrl;
    public AbstractConstruction taskConstruction;

    public int bringMaterialCount = 0;
    public int takeProductCount = 0;
    public float bringMaterialTimer = 0f;
    public float takeProductTimer = 0f;

    [SerializeField] protected List<TaskType> tasks;

    protected override void Awake()
    {
        base.Awake();
        this.DisableTasks();
    }

    protected override void OnEnable()
    {
        TimeManager.OnDayStart += GoWork;
        TimeManager.OnNightStart += GoHome;
    }

    protected override void OnDisable()
    {
        TimeManager.OnDayStart -= GoWork;
        TimeManager.OnNightStart -= GoHome;
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrl();
        this.LoadTasks();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected virtual void LoadWorkerCtrl()
    {
        if (this.workerCtrl != null) return;
        this.workerCtrl = GetComponent<WorkerCtrl>();
        //Debug.Log(transform.name + ": LoadWorkerCtrl", gameObject);
    }

    protected virtual void LoadTasks()
    {
        if (this.taskWorking != null) return;
        Transform tasksObj = transform.Find("Tasks");
        this.taskWorking = tasksObj.GetComponentInChildren<TaskWorking>();
        this.taskGoHome = tasksObj.GetComponentInChildren<TaskGoHome>();
        //Debug.Log(transform.name + ": LoadTasks", gameObject);
    }

    protected virtual void DisableTasks()
    {
        this.taskWorking.gameObject.SetActive(false);
        this.taskGoHome.gameObject.SetActive(false);
    }

    public virtual void GoHome()
    {
        if (this.taskGoHome.gameObject.activeSelf) return;

        this.ClearAllTasks();
        this.taskBuildingCtrl = null;
        this.taskTarget = null;
        this.readyForTask = false;

        this.taskWorking.gameObject.SetActive(false);
        this.taskGoHome.gameObject.SetActive(true);

        this.taskGoHome.GoOutBuilding();
        this.workerCtrl.workerMovement.SetMovingType(MovingType.walking);
        this.TaskAdd(TaskType.goToHome);
    }

    public virtual void GoWork()
    {
        if (this.taskWorking.gameObject.activeSelf) return;

        this.ClearAllTasks();
        this.taskBuildingCtrl = null;
        this.taskTarget = null;
        this.readyForTask = false;

        this.taskGoHome.gameObject.SetActive(false);
        this.taskWorking.gameObject.SetActive(true);

        this.taskWorking.GoOutBuilding();
        this.workerCtrl.workerMovement.SetMovingType(MovingType.walking);
        this.TaskAdd(TaskType.goToWorkStation);
    }

    public virtual void TaskAdd(TaskType taskType)
    {
        TaskType currentTask = this.TaskCurrent();
        if (taskType == currentTask) return;
        this.tasks.Add(taskType);
    }

    public virtual void TaskCurrentDone()
    {
        if (this.tasks.Count <= 0) return;
        this.tasks.RemoveAt(this.tasks.Count - 1);
        this.workerCtrl.workerMovement.SetTarget(null);
    }

    public virtual void ClearAllTasks()
    {
        this.tasks.Clear();
    }

    public virtual TaskType TaskCurrent()
    {
        if (this.tasks.Count <= 0) return TaskType.none;
        return this.tasks[this.tasks.Count - 1];
    }
}
