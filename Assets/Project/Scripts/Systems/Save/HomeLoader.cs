using UnityEngine;

public class HomeLoader : MonoBehaviour
{
    public static HomeLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    private void Start()
    {
        if (SaveManager.Instance.CurrentData != null)
        {
            AchievementManager.Instance.RestoreProgressFromSave(
                SaveManager.Instance.CurrentData.city.achievements
            );
        }
    }
}
