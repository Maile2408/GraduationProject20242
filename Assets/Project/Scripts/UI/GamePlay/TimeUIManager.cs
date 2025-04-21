using UnityEngine;

public class TimeUIManager : MonoBehaviour
{
    [SerializeField] private UnityEngine.GameObject dayIcon;
    [SerializeField] private UnityEngine.GameObject nightIcon;

    private void OnEnable()
    {
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
        GameMessage.Info("A new day has begun!");
    }

    private void HandleNightStart()
    {
        ShowNightIcon();
        GameMessage.Info("Nightfall: All workers have returned home to rest.");
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
