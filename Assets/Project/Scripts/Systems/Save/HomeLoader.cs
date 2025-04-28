using UnityEngine;

public class HomeLoader : MonoBehaviour
{
    public static HomeLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        if (PlayFabAccountManager.Instance.IsLoggedIn)
        {
            HomeController.Instance?.UpdateState();
        }
        else if (RememberMeManager.IsRemembered())
        {
            PlayFabLoginFlow.Instance.TryAutoLogin(
                onSuccess: () =>
                {
                    SaveManager.Instance.DownloadAndApplyFromCloud(() =>
                    {
                        PlayFabProfileManager.Instance.LoadProfile(
                            onSuccess: () =>
                            {
                                RestoreAchievements();
                                HomeController.Instance?.UpdateState();
                            },
                            onError: msg => Debug.LogWarning("Profile load failed: " + msg)
                        );
                    });
                },
                onFail: msg =>
                {
                    Debug.Log("[HomeLoader] AutoLogin skipped: " + msg);
                });
        }
        else
        {
            HomeController.Instance?.UpdateState();
        }
    }

    private void RestoreAchievements()
    {
        AchievementManager.Instance.Reset();
        AchievementManager.Instance.RestoreProgressFromSave(
            SaveManager.Instance.CurrentData.city.achievements
        );
    }
}
