using System;
using UnityEngine;

public class PlayFabLoginFlow : MonoBehaviour
{
    public static PlayFabLoginFlow Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoginAndLoadData(string email, string password, Action onComplete, Action<string> onError)
    {
        ResetAll();

        PlayFabAccountManager.Instance.Login(email, password,
            onSuccess: () =>
            {
                PlayFabProfileManager.Instance.LoadProfile(
                    onSuccess: () =>
                    {
                        PlayFabManager.Instance.DownloadUserData(saveData =>
                        {
                            if (saveData != null)
                            {
                                SaveManager.Instance.CurrentData = saveData;
                                SaveManager.Instance.SaveToDisk();

                                AchievementManager.Instance.Reset();
                                AchievementManager.Instance.RestoreProgressFromSave(saveData.city.achievements);

                                onComplete?.Invoke();
                            }
                            else
                            {
                                var defaultData = SaveDataFactory.CreateDefaultSaveData(
                                    PlayFabAccountManager.Instance.PlayFabId,
                                    PlayFabProfileManager.Instance.Username,
                                    PlayFabProfileManager.Instance.CityName,
                                    PlayFabProfileManager.Instance.CharacterType
                                );

                                SaveManager.Instance.CurrentData = defaultData;
                                SaveManager.Instance.SaveAndUpload();

                                AchievementManager.Instance.Reset();
                                onComplete?.Invoke();
                            }
                        });
                    },
                    onError: error =>
                    {
                        onError?.Invoke("Login OK, but failed to load profile: " + error);
                    });
            },
            onError: error =>
            {
                onError?.Invoke("Login failed: " + error);
            });
    }

    public void TryAutoLogin(Action onSuccess, Action<string> onFail)
    {
        if (!RememberMeManager.IsRemembered())
        {
            onFail?.Invoke("No remembered login found.");
            return;
        }

        string email = RememberMeManager.GetEmail();
        string password = RememberMeManager.GetPassword();

        LoginAndLoadData(email, password,
            onComplete: onSuccess,
            onError: error =>
            {
                Debug.LogWarning("[AutoLogin] Failed: " + error);
                RememberMeManager.Clear();
                onFail?.Invoke(error);
            });
    }

    public void Logout()
    {
        PlayFabAccountManager.Instance?.Logout();
        PlayFabProfileManager.Instance?.Reset();

        SaveManager.Instance.CurrentData = null;
        AchievementManager.Instance?.Reset();
        RememberMeManager.Clear();

        //Debug.Log("[PlayFabLoginFlow] Logout completed.");
    }

    private void ResetAll()
    {
        PlayFabAccountManager.Instance?.Logout();
        PlayFabProfileManager.Instance?.Reset();

        SaveManager.Instance.CurrentData = null;
        AchievementManager.Instance?.Reset();

        //Debug.Log("[PlayFabLoginFlow] Reset all state before login.");
    }
}
