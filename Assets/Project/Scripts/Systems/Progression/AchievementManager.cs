using UnityEngine;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    private Dictionary<string, AchievementProgress> progressDict = new();

    private void Awake()
    {
        Instance = this;

        var achievementArray = Resources.LoadAll<AchievementData>(PoolPrefabPath.Achievement(""));
        foreach (var data in achievementArray)
        {
            if (progressDict.ContainsKey(data.id))
            {
                Debug.LogWarning($"Duplicate achievement ID: {data.id}");
                continue;
            }

            progressDict[data.id] = new AchievementProgress(data);
        }
    }

    public void ReportProgress(AchievementType type, int amount)
    {
        foreach (var progress in progressDict.Values)
        {
            if (progress.data.type != type || progress.isCompleted) continue;

            bool completed = progress.AddProgress(amount);
            if (completed)
            {
                CityLevelManager.Instance.AddXP(progress.data.rewardXP);
                CurrencyManager.Instance.AddCoin(progress.data.rewardCoin);
                GameMessage.Success($"{progress.data.title} (+{progress.data.rewardXP} XP, +{progress.data.rewardCoin} Coin)");
            }
        }
    }

    public List<AchievementProgress> GetAllProgress() => new(progressDict.Values);
}
