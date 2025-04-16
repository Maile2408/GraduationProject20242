using TMPro;
using UnityEngine;

public class GamePlayController : MonoBehaviour, IKeyBack
{
    public const string NAME = "GamePlay";

    [SerializeField] TextMeshProUGUI workerAmount;
    [SerializeField] TextMeshProUGUI coinAmount;

    private void OnEnable()
    {
        WorkerManager.OnWorkerListChanged += UpdateWorkerDisplay;
        CurrencyManager.OnCoinChanged += UpdateCoinDisplay;
        UpdateCoinDisplay();
        UpdateWorkerDisplay();
    }

    private void OnDisable()
    {
        WorkerManager.OnWorkerListChanged -= UpdateWorkerDisplay;
        CurrencyManager.OnCoinChanged -= UpdateCoinDisplay;
    }

    public void UpdateWorkerDisplay()
    {
        if (WorkerManager.Instance == null || workerAmount == null) return;
        var workers = WorkerManager.Instance.WorkerCtrls();
        workerAmount.text = $"{workers.Count}";
    }

    public void UpdateCoinDisplay()
    {
        if (CurrencyManager.Instance == null || coinAmount == null) return;
        var coins = CurrencyManager.Instance.CurrentCoin();
        coinAmount.text = $"{coins}";
    }

    public void OnProfileMenuButtonTap()
    {
        ScreenManager.Add<ProfileMenuController>(ProfileMenuController.NAME);
    }

    public void OnSettingsButtonTap()
    {
        ScreenManager.Add<SettingsController>(SettingsController.NAME);
    }

    public void OnHelpButtonTap()
    {

    }

    public void OnDestroyButtonTap()
    {
        DestroyManager.Instance.EnterDestroyMode();
    }

    public void OnPlusWorkerButtonTap()
    {
        ScreenManager.Add<WorkersController>(WorkersController.NAME);
    }

    public void OnPlusCoinButtonTap()
    {
        var buildings = BuildingManager.Instance.BuildingCtrls();
        int totalCollected = 0;

        foreach (var building in buildings)
        {
            if (building.TryGetComponent(out TaxBuildingCtrl tax) && tax.IsReadyToCollect())
            {
                int coin = tax.Collect(); 
                totalCollected += coin;
            }
        }

        if (totalCollected > 0)
        {
            CityLevelManager.Instance.AddXP(totalCollected);
            GameMessage.Success($"Collected total {totalCollected} coins! +{totalCollected} XP");

            //Achievement
            AchievementReporter.CollectTax(totalCollected);
        }
        else
        {
            GameMessage.Warning("No buildings are ready to collect tax.");
        }
    }

    public void OnBuildMenuButtonTap()
    {
        ScreenManager.Add<BuildMenuController>(BuildMenuController.NAME);
    }

    public void OnStorageButtonTap()
    {
        ScreenManager.Add<StorageController>(StorageController.NAME);
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}