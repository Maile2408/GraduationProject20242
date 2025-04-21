using System.Collections.Generic;
using UnityEngine;

public class AchievementRewardsController : MonoBehaviour, IKeyBack
{
    public const string NAME = "AchievementRewards";

    [SerializeField] private Transform content;

    private void OnEnable()
    {
        AudioManager.Instance.PlayReward();
        RefreshUI();
    }

    private void RefreshUI()
    {
        foreach (Transform child in content)
        {
            PoolManager.Instance.Despawn(child.gameObject);
        }

        List<AchievementProgress> list = AchievementManager.Instance.GetAllProgress();

        foreach (var progress in list)
        {
            if (progress.isCompleted && !progress.isRewardClaimed)
            {
                UnityEngine.GameObject slot = PoolManager.Instance.Spawn(PoolPrefabPath.UI("AchievementSlot"), content);
                slot.GetComponent<AchievementSlotUI>().Setup(progress);
            }
        }
    }

    public void OnClaimButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        AchievementManager.Instance.ClaimAllPendingRewards();
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}