using UnityEngine;

public class HomeLoader : MonoBehaviour
{
    public static HomeLoader Instance;

    public static bool IsReadyToPlay = false;

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
        IsReadyToPlay = false;

        if (RememberMeManager.IsRemembered())
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
                                IsReadyToPlay = true;
                            },
                            onError: msg =>
                            {
                                Debug.LogWarning("Profile load failed: " + msg);
                                IsReadyToPlay = false;
                            });
                    });
                },
                onFail: msg =>
                {
                    Debug.Log("[HomeLoader] AutoLogin skipped: " + msg);
                    IsReadyToPlay = false;
                });
        }
        else
        {
            if (SaveManager.Instance.CurrentData != null) RestoreAchievements();
            IsReadyToPlay = true;
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
