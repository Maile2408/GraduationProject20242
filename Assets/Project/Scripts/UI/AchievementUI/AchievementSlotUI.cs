using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementSlotUI : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI textTitle;
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI textDescription;
    [SerializeField] private TextMeshProUGUI textProgress;

    public void Setup(AchievementProgress progress)
    {
        var data = progress.data;

        textTitle.text = data.title;
        iconImage.sprite = data.icon;
        textDescription.text = data.description;

        if (progress.isCompleted)
            textProgress.text = "<color=green>✔ Completed</color>";
        else
            textProgress.text = $"{progress.current}/{data.goalAmount}";
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
