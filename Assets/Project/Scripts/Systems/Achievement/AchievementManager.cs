using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance;

    private Dictionary<string, AchievementProgress> progressDict = new();
    private List<AchievementProgress> progressList = new();
    private List<AchievementProgress> pendingClaimQueue = new();
    private bool isPopupPending = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        LoadAllAchievements();
    }

    private void LoadAllAchievements()
    {
        var achievementArray = Resources.LoadAll<AchievementData>("Achievements/");
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

            bool completed = false;

            if (AchievementProgress.IsCumulativeType(type))
                completed = progress.AddProgress(amount);
            else if (amount > progress.current)
                completed = progress.SetProgress(amount);

            if (completed)
            {
                if (!progress.isRewardClaimed && !pendingClaimQueue.Contains(progress))
                {
                    pendingClaimQueue.Add(progress);
                    ShowAchievementPopup();
                }
            }
        }
    }

    private void ShowAchievementPopup()
    {
        if(isPopupPending) return;
        
        isPopupPending = true;
        Invoke(nameof(OpenPopup), 1.5f);
    }

    private void OpenPopup()
    {
        isPopupPending = false;
        if(pendingClaimQueue.Count > 0 ) ScreenManager.Add<AchievementRewardsController>(AchievementRewardsController.NAME);
    }

    public void ClaimAllPendingRewards()
    {
        int totalXP = 0;
        int totalCoin = 0;

        foreach (var progress in pendingClaimQueue)
        {
            if (progress.isRewardClaimed) continue;

            totalXP += progress.data.rewardXP;
            totalCoin += progress.data.rewardCoin;
            progress.isRewardClaimed = true;
        }

        if (totalXP > 0) CityLevelManager.Instance.AddXP(totalXP);
        if (totalCoin > 0) CurrencyManager.Instance.AddCoin(totalCoin);

        GameMessage.Success($"Claimed all achievements! (+{totalXP} XP, +{totalCoin} Coin)");

        pendingClaimQueue.Clear();
    }

    public int GetCurrentProgress(AchievementType type)
    {
        int max = 0;
        foreach (var progress in progressList)
        {
            if (progress.data.type != type) continue;
            if (progress.current > max)
                max = progress.current;
        }
        return max;
    }

    public List<AchievementProgress> GetAllProgress() => new(progressList);

    public bool IsCompleted(string achievementId)
    {
        if (!progressDict.ContainsKey(achievementId)) return false;
        return progressDict[achievementId].isCompleted;
    }
}
