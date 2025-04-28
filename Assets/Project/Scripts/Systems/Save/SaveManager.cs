using UnityEngine;
using System;
using System.Collections;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public UserSaveData CurrentData = new();

    private const float AutoSaveInterval = 60f;
    private Coroutine autoSaveCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }

        Instance = this;

        LoadFromDisk();
        autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
    }

    private void OnDestroy()
    {
        if (autoSaveCoroutine != null)
        {
            StopCoroutine(autoSaveCoroutine);
            autoSaveCoroutine = null;
        }
    }

    public void SaveToDisk()
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        string key = GetPlayerSaveKey();
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    public void LoadFromDisk()
    {
        string key = GetPlayerSaveKey();
        if (!PlayerPrefs.HasKey(key)) return;

        string json = PlayerPrefs.GetString(key);
        CurrentData = JsonUtility.FromJson<UserSaveData>(json);
    }

    public void SaveAndUpload()
    {
        SaveToDisk();
        PlayFabManager.Instance.UploadUserData(CurrentData);

        var city = CurrentData.city;

        PlayFabLeaderboardManager.Instance.UpdateLeaderboard(
            city.cityLevel,
            city.xp,
            city.buildings.Count,
            city.workers.Count,
            Mathf.RoundToInt(city.coin)
        );
    }

    public void ClearLocalSave()
    {
        string key = GetPlayerSaveKey();
        if (PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.DeleteKey(key);
            PlayerPrefs.Save();
        }
    }

    public void DownloadAndApplyFromCloud(Action onDone = null)
    {
#if USE_PLAYFAB
        PlayFabManager.Instance.DownloadUserData(data =>
        {
            if (data != null)
            {
                CurrentData = data;
                SaveToDisk();
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

            if (SaveStateCollector.Instance != null)
            {
                SaveStateCollector.Instance.SaveAll();
                SaveToDisk();
                //Debug.Log("[AutoSave] Game auto-saved at " + Time.time);
            }
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
