using System.Collections;
using TMPro;
using UnityEngine;

public class GamePlayController : MonoBehaviour, IKeyBack
{
    public const string NAME = "GamePlay";

    public static GamePlayController Instance;

    [SerializeField] TextMeshProUGUI workerAmount;
    [SerializeField] TextMeshProUGUI coinAmount;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

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
        var coins = CurrencyManager.Instance.Coin;
        coinAmount.text = $"{coins}";
    }

    public void OnProfileMenuButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<ProfileMenuController>(ProfileMenuController.NAME);
    }

    public void OnSettingsButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<SettingsController>(SettingsController.NAME);
    }

    public void OnHelpButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<HelpController>(HelpController.NAME);
    }

    public void OnDestroyButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        DestroyManager.Instance.EnterDestroyMode();
    }

    public void OnPlusWorkerButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<WorkersController>(WorkersController.NAME);
    }

    public void OnPlusCoinButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();

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
            AudioManager.Instance.PlayTaxCollect();

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
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<BuildMenuController>(BuildMenuController.NAME);
    }

    public void OnStorageButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Add<StorageController>(StorageController.NAME);
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}