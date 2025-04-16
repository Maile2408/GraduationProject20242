using UnityEngine;
using System;

public class TimeManager : SaiBehaviour
{
    public static TimeManager Instance { get; private set; }

    public enum TimeState { Day, Night }

    [SerializeField] private TimeState currentTime = TimeState.Day;
    public TimeState CurrentTime => currentTime;

    public bool IsDay => currentTime == TimeState.Day;
    public bool IsNight => currentTime == TimeState.Night;

    [Header("Time Config")]
    [SerializeField] private float dayDuration = 300f;    
    [SerializeField] private float nightDuration = 120f;
    [SerializeField] private float timer;

    public static event Action OnDayStart;
    public static event Action OnNightStart;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        if (IsDay) OnDayStart?.Invoke();
        else OnNightStart?.Invoke();
    }

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (IsDay && timer >= dayDuration)
        {
            //Achievement
            AchievementReporter.PlayTime(dayDuration);
            
            timer = 0f;
            SwitchToNight();
        }
        else if (IsNight && timer >= nightDuration)
        {
            //Achievement
            AchievementReporter.PlayTime(nightDuration);
            
            timer = 0f;
            SwitchToDay();
        }
    }

    private void SwitchToDay()
    {
        currentTime = TimeState.Day;
        OnDayStart?.Invoke();
        Debug.Log("[TimeManager] Switched to DAY");
    }

    private void SwitchToNight()
    {
        currentTime = TimeState.Night;
        OnNightStart?.Invoke();
        Debug.Log("[TimeManager] Switched to NIGHT");
    }

    public void ForceSetTime(TimeState newTime)
    {
        if (currentTime == newTime) return;

        currentTime = newTime;
        timer = 0f;

        if (IsDay) OnDayStart?.Invoke();
        else OnNightStart?.Invoke();

        Debug.Log($"[TimeManager] Forced set to: {newTime}");
    }
}
