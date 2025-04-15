using UnityEngine;
using System.Collections.Generic;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    private Dictionary<string, AchievementProgress> progressDict = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ✅ giữ khi chuyển scene

        LoadAllAchievements();
    }

    private void LoadAllAchievements()
    {
        var achievementArray = Resources.LoadAll<AchievementData>(PoolPrefabPath.Achievement(""));
        foreach (var data in achievementArray)
        {
            if (progressDict.ContainsKey(data.id))
            {
                Debug.LogWarning($"[AchievementManager] Duplicate ID: {data.id}");
                continue;
            }

            progressDict[data.id] = new AchievementProgress(data);
        }

        Debug.Log($"[AchievementManager] Loaded {progressDict.Count} achievements");
    }

    public void ReportProgress(AchievementType type, int amount)
    {
        foreach (var progress in progressDict.Values)
        {
            if (progress.data.type != type || progress.isCompleted) continue;

            bool completed = progress.AddProgress(amount);
            if (completed)
            {
                CityLevelManager.Instance?.AddXP(progress.data.rewardXP);
                CurrencyManager.Instance?.AddCoin(progress.data.rewardCoin);

                GameMessage.Success($"🏆 {progress.data.title} (+{progress.data.rewardXP} XP, +{progress.data.rewardCoin}💰)");

                // 👉 Tùy chọn: hiển thị popup khi đạt được
                // var popup = PoolManager.Instance.Spawn(...);
                // popup.GetComponent<AchievementPopupController>().Show(progress.data);
            }
        }
    }

    public List<AchievementProgress> GetAllProgress() => new(progressDict.Values);

    public bool IsCompleted(string achievementId)
    {
        if (!progressDict.ContainsKey(achievementId)) return false;
        return progressDict[achievementId].isCompleted;
    }
}
