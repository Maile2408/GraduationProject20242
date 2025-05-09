using UnityEngine;

public class TimeUIManager : MonoBehaviour
{
    [SerializeField] private GameObject dayIcon;
    [SerializeField] private GameObject nightIcon;

    private bool hasHandledFirst = false;

    private void OnEnable()
    {
        hasHandledFirst = false;
        TimeManager.OnDayStart += HandleDayStart;
        TimeManager.OnNightStart += HandleNightStart;
    }

    private void OnDisable()
    {
        TimeManager.OnDayStart -= HandleDayStart;
        TimeManager.OnNightStart -= HandleNightStart;
    }

    private void Start()
    {
        UpdateIconSilently();
    }

    private void HandleDayStart()
    {
        ShowDayIcon();
        if (hasHandledFirst) GameMessage.Info("A new day has begun!");
        hasHandledFirst = true;
    }

    private void HandleNightStart()
    {
        ShowNightIcon();
        if (hasHandledFirst) GameMessage.Info("Nightfall: All workers have returned home to rest.");
        hasHandledFirst = true;
    }

    private void UpdateIconSilently()
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
