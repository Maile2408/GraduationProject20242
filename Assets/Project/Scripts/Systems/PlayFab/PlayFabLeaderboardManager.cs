using System;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabLeaderboardManager : MonoBehaviour
{
    public static PlayFabLeaderboardManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void UpdateLeaderboard(int cityLevel, int xp, int buildingCount, int workerCount, int coin)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate { StatisticName = "cityLevel", Value = cityLevel },
                new StatisticUpdate { StatisticName = "xp", Value = xp },
                new StatisticUpdate { StatisticName = "buildingCount", Value = buildingCount },
                new StatisticUpdate { StatisticName = "workerCount", Value = workerCount },
                new StatisticUpdate { StatisticName = "coin", Value = coin }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request,
            result => Debug.Log("Leaderboard updated successfully."),
            error => Debug.LogError("UpdateLeaderboard failed: " + error.GenerateErrorReport()));
    }

    public void GetLeaderboard(string statName, Action<List<PlayerLeaderboardEntry>> onSuccess, Action<string> onError = null)
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = statName,
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request,
            result => onSuccess?.Invoke(result.Leaderboard),
            error =>
            {
                Debug.LogError("GetLeaderboard failed: " + error.GenerateErrorReport());
                onError?.Invoke(error.ErrorMessage);
            });
    }

    public void GetUserData(string playFabId, Action<Dictionary<string, UserDataRecord>> onSuccess, Action<string> onError = null)
    {
        var request = new GetUserDataRequest
        {
            PlayFabId = playFabId
        };

        PlayFabClientAPI.GetUserData(request,
            result => onSuccess?.Invoke(result.Data),
            error =>
            {
                Debug.LogWarning("GetUserData failed: " + error.GenerateErrorReport());
                onError?.Invoke(error.ErrorMessage);
            });
    }
}
