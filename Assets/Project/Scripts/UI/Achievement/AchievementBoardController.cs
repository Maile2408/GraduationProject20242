using System.Collections.Generic;
using UnityEngine;

public class AchievementBoardController : MonoBehaviour, IKeyBack
{
    public const string NAME = "AchievementBoard";

    [SerializeField] private Transform content;

    private void OnEnable()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        foreach (Transform child in content)
        {
            PoolManager.Instance.Despawn(child.gameObject);
        }

        List<AchievementProgress> list = AchievementManager.Instance.GetAllProgress();

        foreach (var progress in list)
        {
            GameObject slot = PoolManager.Instance.Spawn(PoolPrefabPath.UI("AchievementSlot"), content);
            slot.GetComponent<AchievementSlotUI>().Setup(progress);
        }
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
