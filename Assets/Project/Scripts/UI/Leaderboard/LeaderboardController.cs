using System.Collections.Generic;
using TMPro;
using UnityEngine;
using PlayFab.ClientModels;

public class LeaderboardController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Leaderboard";

    [Header("UI")]
    [SerializeField] private TMP_Dropdown dropdownStatType;
    [SerializeField] private Transform content;

    private readonly List<UnityEngine.GameObject> activeSlots = new();

    private readonly Dictionary<string, string> statMapping = new()
    {
        { "City Level", "cityLevel" },
        { "XP", "xp" },
        { "Coin", "coin" },
        { "Building", "buildingCount" },
        { "Worker", "workerCount" }
    };

    private void OnEnable()
    {
        dropdownStatType.onValueChanged.AddListener(OnDropdownChanged);
        LoadLeaderboard(dropdownStatType.options[dropdownStatType.value].text);
    }

    private void OnDisable()
    {
        dropdownStatType.onValueChanged.RemoveListener(OnDropdownChanged);
        DespawnAllSlots();
    }

    private void OnDropdownChanged(int index)
    {
        string label = dropdownStatType.options[index].text;
        LoadLeaderboard(label);
    }

    private void LoadLeaderboard(string label)
    {
        if (!statMapping.TryGetValue(label, out string statName))
        {
            Debug.LogWarning("Invalid stat label: " + label);
            return;
        }

        PlayFabLeaderboardManager.Instance.GetLeaderboard(statName, OnReceivedLeaderboard);
    }

    private void OnReceivedLeaderboard(List<PlayerLeaderboardEntry> entries)
    {
        DespawnAllSlots();

        foreach (var entry in entries)
        {
            UnityEngine.GameObject go = PoolManager.Instance.Spawn(PoolPrefabPath.UI("LeaderboardSlot"), content);
            activeSlots.Add(go);

            var slot = go.GetComponent<LeaderboardSlot>();

            int rank = entry.Position + 1;
            int score = entry.StatValue;

            slot.SetInfo(rank, "Loading...", "Loading...", score);

            string playFabId = entry.PlayFabId;

            PlayFabLeaderboardManager.Instance.GetUserData(playFabId,
                data =>
                {
                    string username = data != null && data.ContainsKey("username") ? data["username"].Value : "Unknown";
                    string cityName = data != null && data.ContainsKey("cityName") ? data["cityName"].Value : "Unknown";
                    slot.SetInfo(rank, username, cityName, score);
                },
                error =>
                {
                    Debug.LogWarning($"Failed to get user data for {playFabId}: {error}");
                    slot.SetInfo(rank, "Unknown", "Unknown", score);
                });
        }
    }

    private void DespawnAllSlots()
    {
        foreach (var obj in activeSlots)
        {
            PoolManager.Instance.Despawn(obj);
        }
        activeSlots.Clear();
    }

    public void OnCloseButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
