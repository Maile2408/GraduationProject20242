using UnityEngine;

public class TimeUIManager : MonoBehaviour
{
    [SerializeField] private GameObject dayIcon;
    [SerializeField] private GameObject nightIcon;

    private void OnEnable()
    {
        TimeManager.OnDayStart += ShowDayIcon;
        TimeManager.OnNightStart += ShowNightIcon;
    }

    private void OnDisable()
    {
        TimeManager.OnDayStart -= ShowDayIcon;
        TimeManager.OnNightStart -= ShowNightIcon;
    }

    private void Start()
    {
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (TimeManager.Instance.IsDay)
            ShowDayIcon();
        else
            ShowNightIcon();
    }

    private void ShowDayIcon()
    {
        if (dayIcon != null) dayIcon.SetActive(true);
        if (nightIcon != null) nightIcon.SetActive(false);
    }

    private void ShowNightIcon()
    {
        if (dayIcon != null) dayIcon.SetActive(false);
        if (nightIcon != null) nightIcon.SetActive(true);
    }
}
