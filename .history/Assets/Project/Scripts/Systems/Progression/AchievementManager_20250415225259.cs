using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    private Dictionary<string, AchievementProgress> progressDict = new();
    private List<AchievementProgress> progressList = new(); 

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadAllAchievements();
    }

    private void LoadAllAchievements()
    {
        var achievementArray = Resources.LoadAll<AchievementData>(PoolPrefabPath.Achievement(""));
        var sorted = achievementArray.OrderBy(a => a.order).ToList();

        foreach (var data in sorted)
        {
            if (string.IsNullOrWhiteSpace(data.id))
            {
                Debug.LogWarning($"[AchievementManager] Skipped achievement with empty ID.");
                continue;
            }

            if (progressDict.ContainsKey(data.id))
            {
                Debug.LogWarning($"[AchievementManager] Duplicate ID: {data.id}");
                continue;
            }

            var progress = new AchievementProgress(data);
            progressDict[data.id] = progress;
            progressList.Add(progress);
        }

        Debug.Log($"[AchievementManager] Loaded {progressDict.Count} achievements");
    }

    public void ReportProgress(AchievementType type, int amount)
    {
        foreach (var progress in progressList)
        {
            if (progress.data.type != type || progress.isCompleted) continue;

            bool completed = progress.AddProgress(amount);
            if (completed)
            {
                CityLevelManager.Instance?.AddXP(progress.data.rewardXP);
                CurrencyManager.Instance?.AddCoin(progress.data.rewardCoin);

                GameMessage.Success($"🏆 {progress.data.title} (+{progress.data.rewardXP} XP, +{progress.data.rewardCoin} Coin)");

                // Optionally: show popup here
                // var popup = PoolManager.Instance.Spawn(...);
            }
        }
    }

    public List<AchievementProgress> GetAllProgress() => new(progressList);

    public bool IsCompleted(string achievementId)
    {
        if (!progressDict.ContainsKey(achievementId)) return false;
        return progressDict[achievementId].isCompleted;
    }
}
