using System;
using UnityEngine;

public class PlayFabLoginFlow : MonoBehaviour
{
    public static PlayFabLoginFlow Instance;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void LoginAndLoadData(string email, string password, Action onComplete, Action<string> onError)
    {
        PlayFabAccountManager.Instance.Login(email, password, () =>
        {
            Debug.Log("[LoginFlow] Login success. Loading profile...");
            PlayFabProfileManager.Instance.LoadProfile(() =>
            {
                Debug.Log("[LoginFlow] Profile loaded. Loading SaveData...");
                PlayFabManager.Instance.DownloadUserData(data =>
                {
                    if (data != null)
                    {
                        SaveManager.Instance.CurrentData = data;
                        SaveManager.Instance.SaveToDisk();
                        Debug.Log("[LoginFlow] SaveData loaded.");
                    }
                    else
                    {
                        Debug.LogWarning("[LoginFlow] No SaveData found. Using default.");
                        SaveManager.Instance.CurrentData = new UserSaveData(); // fallback
                    }

                    onComplete?.Invoke();
                });
            }, error =>
            {
                Debug.LogError("[LoginFlow] Failed to load profile: " + error);
                onError?.Invoke(error);
            });

        }, error =>
        {
            Debug.LogError("[LoginFlow] Login failed: " + error);
            onError?.Invoke(error);
        });
    }
}
