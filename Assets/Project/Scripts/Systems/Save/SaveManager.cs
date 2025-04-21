using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public UserSaveData CurrentData = new();

    private const float AutoSaveInterval = 60f;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        LoadFromDisk();
        StartCoroutine(AutoSaveRoutine());
    }

    // Save current data to PlayerPrefs (local device)
    public void SaveToDisk()
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        string key = GetPlayerSaveKey();
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();

        Debug.Log("[SaveManager] Saved to disk as " + key);
    }

    // Load saved data from PlayerPrefs
    public void LoadFromDisk()
    {
        string key = GetPlayerSaveKey();
        if (!PlayerPrefs.HasKey(key)) return;

        string json = PlayerPrefs.GetString(key);
        CurrentData = JsonUtility.FromJson<UserSaveData>(json);

        Debug.Log("[SaveManager] Loaded from disk: " + key);
    }

    // Save + Upload to Playfab cloud
    public void SaveAndUpload()
    {
        SaveToDisk();
#if USE_PLAYFAB
        PlayfabManager.Instance.UploadUserData(CurrentData);
#endif
    }

    // Download from cloud and replace current data
    public void DownloadAndApplyFromCloud(Action onDone = null)
    {
#if USE_PLAYFAB
        PlayfabManager.Instance.DownloadUserData(data =>
        {
            if (data != null)
            {
                CurrentData = data;
                SaveToDisk();
                Debug.Log("[SaveManager] Data applied from cloud");
            }
            onDone?.Invoke();
        });
#else
        onDone?.Invoke();
#endif
    }

    private void OnApplicationQuit()
    {
        SaveToDisk();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) SaveToDisk();
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(AutoSaveInterval);
            SaveStateCollector.Instance?.SaveAll();
            SaveToDisk();
            Debug.Log("[AutoSave] Game auto-saved at " + Time.time);
        }
    }

    private string GetPlayerSaveKey()
    {
#if USE_PLAYFAB
        string id = PlayFabAccountManager.Instance?.PlayfabID;
        return string.IsNullOrEmpty(id) ? "save_data_guest" : $"save_data_{id}";
#else
        return "save_data_local";
#endif
    }
}
