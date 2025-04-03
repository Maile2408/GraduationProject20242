using TMPro;
using UnityEngine;

public class GamePlayController : MonoBehaviour, IKeyBack
{
    public const string NAME = "GamePlay";

    [SerializeField] TextMeshProUGUI workerAmount;
    [SerializeField] TextMeshProUGUI coinAmount;

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