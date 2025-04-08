using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WorkersController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Workers";

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI txtCurrentWorkers;
    [SerializeField] private TextMeshProUGUI txtWorkerCapacity;
    [SerializeField] private TextMeshProUGUI txtEmployedWorkers;
    [SerializeField] private TextMeshProUGUI txtJobSlots;

    private void OnEnable()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var workers = WorkerManager.Instance.WorkerCtrls();

        txtCurrentWorkers.text = $"Current Workers: {workers.Count}";
        txtEmployedWorkers.text = $"Employed Workers: {CountEmployed(workers)}";
        txtWorkerCapacity.text = $"Worker Capacity: {GetTotalCapacity(BuildingTaskType.home)}";
        txtJobSlots.text = $"Total Job Slots: {GetTotalCapacity(BuildingTaskType.workStation)}";
    }

    private int CountEmployed(List<WorkerCtrl> workers)
    {
        int count = 0;
        foreach (var worker in workers)
        {
            if (worker.workerBuildings.GetWork() != null) count++;
        }
        return count;
    }

    private int GetTotalCapacity(BuildingTaskType type)
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

    public void OnHireButtonTap()
    {
        WorkerManager.Instance.StartPlacingWorker();
        ScreenManager.Close();
    }

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}