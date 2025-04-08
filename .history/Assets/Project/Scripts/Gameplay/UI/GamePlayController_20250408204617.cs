using TMPro;
using UnityEngine;

public class GamePlayController : MonoBehaviour, IKeyBack
{
    public const string NAME = "GamePlay";

    [SerializeField] TextMeshProUGUI workerAmount;
    [SerializeField] TextMeshProUGUI coinAmount;
    [SerializeField] Transform coinHUDTarget;

    private void OnEnable()
    {
        WorkerManager.OnWorkerListChanged += UpdateWorkerDisplay;
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.onCoinChanged.AddListener(UpdateCoinDisplay);
        }
    }

    private void OnDisable()
    {
        WorkerManager.OnWorkerListChanged -= UpdateWorkerDisplay;
        CurrencyManager.Instance.onCoinChanged.RemoveListener(UpdateCoinDisplay);
    }

    private void Start()
    {
        TaxCoinEffect.SetTargetUI(coinHUDTarget);
        UpdateCoinDisplay(CurrencyManager.Coin);
    }

    public void UpdateWorkerDisplay()
    {
        var workers = WorkerManager.Instance.WorkerCtrls();
        workerAmount.text = $"{workers.Count}";
    }

    public void UpdateCoinDisplay(float newCoin)
    {
        coinAmount.text = $"{newCoin:0}";
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