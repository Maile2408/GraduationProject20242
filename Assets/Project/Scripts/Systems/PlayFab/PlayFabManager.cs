using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager Instance;

    private const string SaveKey = "UserSaveData";

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void UploadUserData(UserSaveData data, Action onSuccess = null, Action<string> onError = null)
    {
        string json = JsonUtility.ToJson(data);

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { SaveKey, json }
            }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result =>
            {
                Debug.Log("[PlayFabManager] User data uploaded successfully.");
                onSuccess?.Invoke();
            },
            error =>
            {
                Debug.LogError("[PlayFabManager] Failed to upload user data: " + error.GenerateErrorReport());
                onError?.Invoke(error.ErrorMessage);
            });
    }

    public void DownloadUserData(Action<UserSaveData> onComplete)
    {
        var request = new GetUserDataRequest();

        PlayFabClientAPI.GetUserData(request,
            result =>
            {
                if (result.Data != null && result.Data.ContainsKey(SaveKey))
                {
                    string json = result.Data[SaveKey].Value;
                    var data = JsonUtility.FromJson<UserSaveData>(json);
                    Debug.Log("[PlayFabManager] User data downloaded successfully.");
                    onComplete?.Invoke(data);
                }
                else
                {
                    Debug.LogWarning("[PlayFabManager] No UserSaveData found in cloud.");
                    onComplete?.Invoke(null);
                }
            },
            error =>
            {
                Debug.LogError("[PlayFabManager] Failed to download user data: " + error.GenerateErrorReport());
                onComplete?.Invoke(null);
            });
    }

    public void DeleteUserData(Action onSuccess = null, Action<string> onError = null)
    {
        var request = new UpdateUserDataRequest
        {
            KeysToRemove = new List<string> { SaveKey }
        };

        PlayFabClientAPI.UpdateUserData(request,
            result =>
            {
                Debug.Log("[PlayFabManager] UserSaveData successfully deleted from cloud.");
                onSuccess?.Invoke();
            },
            error =>
            {
                Debug.LogError("[PlayFabManager] Failed to delete user data: " + error.GenerateErrorReport());
                onError?.Invoke(error.ErrorMessage);
            });
    }
}
