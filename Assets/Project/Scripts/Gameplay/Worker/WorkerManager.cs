using System;
using System.Collections.Generic;
using UnityEngine;

public class WorkerManager : SaiBehaviour
{
    public static WorkerManager Instance;

    [Header("Data")]
    [SerializeField] protected List<WorkerCtrl> workerCtrls = new();
    [SerializeField] private float workerCost = 100f;

    [Header("Placement")]
    [SerializeField] private LayerMask groundLayer;
    private bool isPlacingWorker = false;
    private WorkerCtrl placingWorker;
    public static event Action OnWorkerListChanged;

    public float WorkerCost() => workerCost;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Debug.LogError("Only 1 WorkerManager allowed");
        }
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        OnWorkerListChanged?.Invoke();

        //Achievement
        AchievementReporter.MaxWorkerReached(this.GetTotalCapacity(BuildingTaskType.home));
        AchievementReporter.UpdateTotalWorker(this.workerCtrls.Count);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkerCtrls();
    }

    protected override void Update()
    {
        base.Update();

        if (isPlacingWorker && placingWorker != null)
        {
            Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 999f, groundLayer))
            {
                placingWorker.transform.position = hit.point;

                if (Input.GetMouseButtonDown(0))
                {
                    FinalizePlacement();
                }

                if (Input.GetMouseButtonDown(1))
                {
                    CancelPlacement();
                }
            }
        }
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

        OnWorkerListChanged?.Invoke();
    }

    public virtual void RemoveWorker(WorkerCtrl workerCtrl)
    {
        this.workerCtrls.Remove(workerCtrl);
    }

    public int GetTotalCapacity(BuildingTaskType type)
    {
        int total = 0;
        foreach (var building in BuildingManager.Instance.BuildingCtrls())
        {
            if (building.buildingTaskType != type) continue;
            if (building.TryGetComponent(out Workers workersComp))
            {
                total += workersComp.MaxWorker();
            }
        }
        return total;
    }

    public int CountEmployed(List<WorkerCtrl> workers)
    {
        int count = 0;
        foreach (var worker in workers)
        {
            if (worker.workerBuildings.GetWork() != null) count++;
        }
        return count;
    }

    public void StartPlacingWorker()
    {
        if (isPlacingWorker) return;

        GameObject prefab = WorkerPrefabCache.GetRandomWorker();
        if (prefab == null)
        {
            Debug.LogError("WorkerManager: Prefab is null.");
            return;
        }

        string path = PoolPrefabPath.Worker(prefab.name);
        GameObject workerGO = PoolManager.Instance.Spawn(path);
        if (workerGO == null)
        {
            Debug.LogError($"WorkerManager: Failed to spawn worker from path: {path}");
            return;
        }

        if (!workerGO.TryGetComponent(out placingWorker))
        {
            Debug.LogError("Worker prefab is missing WorkerCtrl component.");
            PoolManager.Instance.Despawn(workerGO);
            return;
        }

        isPlacingWorker = true;

        GameMessage.Guide("Click on Ground to place a worker");

        Debug.Log("Click on Ground to place a worker.");
    }

    private void FinalizePlacement()
    {
        AddWorker(placingWorker);
        SaveUtils.AssignID(placingWorker.gameObject);
        placingWorker.workerType = SaveUtils.GetPrefabName(placingWorker.gameObject);

        AudioManager.Instance.PlayWorkerSpawn();
        CurrencyManager.Instance.SpendCoin(workerCost);
        CityLevelManager.Instance.AddXP(75);
        GameMessage.Info("New Worker Hired! +75 XP");

        // Achievement
        AchievementReporter.HireWorker();
        AchievementReporter.MaxWorkerReached(this.GetTotalCapacity(BuildingTaskType.home));
        AchievementReporter.UpdateTotalWorker(this.workerCtrls.Count);

        if (TimeManager.Instance.IsDay) placingWorker.workerTasks.GoWork();
        else placingWorker.workerTasks.GoHome();

        placingWorker = null;
        isPlacingWorker = false;
    }

    private void CancelPlacement()
    {
        if (placingWorker != null)
        {
            PoolManager.Instance.Despawn(placingWorker.gameObject);
            placingWorker = null;
        }

        isPlacingWorker = false;
        Debug.Log("Cancelled worker placement.");
    }
}