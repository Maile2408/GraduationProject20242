using System.Collections;
using UnityEngine;

public class HomeLoader : MonoBehaviour
{
    public static HomeLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator LoadProfileData()
    {
        if (PlayFabAccountManager.Instance.IsLoggedIn)
        {
            yield break;
        }

        if (RememberMeManager.IsRemembered())
        {
            bool isDone = false;

            PlayFabLoginFlow.Instance.TryAutoLogin(
                onSuccess: () =>
                {
                    SaveManager.Instance.DownloadAndApplyFromCloud(() =>
                    {
                        PlayFabProfileManager.Instance.LoadProfile(
                            onSuccess: () =>
                            {
                                RestoreAchievements();
                                isDone = true;
                            },
                            onError: msg => isDone = true
                        );
                    });
                },
                onFail: msg =>
                {
                    Debug.Log("[HomeLoader] AutoLogin failed: " + msg);
                    isDone = true;
                });

            while (!isDone)
                yield return null;
        }

        yield return null;
    }

    private void RestoreAchievements()
    {
        AchievementManager.Instance.Reset();
        AchievementManager.Instance.RestoreProgressFromSave(
            SaveManager.Instance.CurrentData.city.achievements
        );
    }
}
