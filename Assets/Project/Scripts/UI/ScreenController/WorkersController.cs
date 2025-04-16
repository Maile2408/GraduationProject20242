using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WorkersController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Workers";

    [Header("UI Text")]
    [SerializeField] private TextMeshProUGUI txtCurrentWorkers;
    [SerializeField] private TextMeshProUGUI txtWorkerCapacity;
    [SerializeField] private TextMeshProUGUI txtEmployedWorkers;
    [SerializeField] private TextMeshProUGUI txtJobSlots;

    [Header("Hire UI")]
    [SerializeField] private Button hireButton;
    [SerializeField] private GameObject note;
    [SerializeField] private GameObject warning;
    [SerializeField] private TextMeshProUGUI txtWarning;

    private bool canHire = false;

    private void OnEnable()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        var workers = WorkerManager.Instance.WorkerCtrls();

        int currentWorkers = workers.Count;
        int workerCapacity = WorkerManager.Instance.GetTotalCapacity(BuildingTaskType.home);
        int countEmployed = WorkerManager.Instance.CountEmployed(workers);
        int totalJobSlots = WorkerManager.Instance.GetTotalCapacity(BuildingTaskType.workStation);

        float coin = CurrencyManager.Instance.CurrentCoin();
        float cost = WorkerManager.Instance.WorkerCost();

        txtCurrentWorkers.text = $"Current Workers: {currentWorkers}";
        txtEmployedWorkers.text = $"Employed Workers: {countEmployed}";
        txtWorkerCapacity.text = $"Worker Capacity: {workerCapacity}";
        txtJobSlots.text = $"Total Job Slots: {totalJobSlots}";

        canHire = currentWorkers < workerCapacity && coin >= cost;

        note.SetActive(true);
        warning.SetActive(false);
    }

    public void OnHireButtonTap()
    {
        if (canHire)
        {
            WorkerManager.Instance.StartPlacingWorker();
            ScreenManager.Close();
        }
        else
        {
            ShowWarningMessage();
        }
    }

    private void ShowWarningMessage()
    {
        var workers = WorkerManager.Instance.WorkerCtrls();
        int currentWorkers = workers.Count;
        int workerCapacity = WorkerManager.Instance.GetTotalCapacity(BuildingTaskType.home);
        
        float coin = CurrencyManager.Instance.CurrentCoin();
        float cost = WorkerManager.Instance.WorkerCost();

        note.SetActive(false);
        warning.SetActive(true);

        if (currentWorkers >= workerCapacity && coin < cost)
            txtWarning.text = "[Not enough coin & Worker capacity full!]";
        else if (currentWorkers >= workerCapacity)
            txtWarning.text = "[Worker capacity reached!]";
        else if (coin < cost)
            txtWarning.text = "[Not enough coin!]";
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