using UnityEngine;

public class HomeLoader : MonoBehaviour
{
    public static HomeLoader Instance;

    public static bool IsReadyToPlay { get; private set; } = false;

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
                    Debug.Log("[HomeLoader] AutoLogin success.");

                    PlayFabProfileManager.Instance.LoadProfile(
                        onSuccess: () =>
                        {
                            Debug.Log("[HomeLoader] Profile loaded.");
                            IsReadyToPlay = true;

                            AchievementManager.Instance.Reset();
                            AchievementManager.Instance.RestoreProgressFromSave(
                                SaveManager.Instance.CurrentData.city.achievements
                            );
                        },
                        onError: msg =>
                        {
                            Debug.LogWarning("Profile load failed: " + msg);
                            IsReadyToPlay = true;
                        });
                },
                onFail: msg =>
                {
                    Debug.Log("[HomeLoader] AutoLogin skipped: " + msg);
                    IsReadyToPlay = true;
                });
        }
        else
        {
            if (SaveManager.Instance.CurrentData != null)
            {
                IsReadyToPlay = true;
                RestoreAchievements();
            }
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
