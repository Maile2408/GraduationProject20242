using UnityEngine;
using System;

public class TimeManager : SaiBehaviour
{
    public static TimeManager Instance { get; private set; }

    public enum TimeState { Day, Night }

    [SerializeField] private TimeState currentTime = TimeState.Day;

    public bool IsDay => currentTime == TimeState.Day;
    public bool IsNight => currentTime == TimeState.Night;

    [Header("Time Config")]
    [SerializeField] private float dayDuration = 300f;
    [SerializeField] private float nightDuration = 90f;
    [SerializeField] private float timer;

    public static event Action OnDayStart;
    public static event Action OnNightStart;

    public TimeState CurrentTime => currentTime;

    public float TimeCounter
    {
        get => timer;
        set => timer = Mathf.Max(0, value);
    }

    public string TimeStateName
    {
        get => currentTime.ToString();
        set
        {
            if (Enum.TryParse(value, out TimeState parsed))
                ForceSetTime(parsed);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private bool hasRestoredTimeState = false;
    protected override void Start()
    {
        base.Start();

        if (!hasRestoredTimeState)
        {
            if (IsDay) OnDayStart?.Invoke();
            else OnNightStart?.Invoke();
        }
    }

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;

        if (IsDay && timer >= dayDuration)
        {
            AchievementReporter.PlayTime(dayDuration);
            timer = 0f;
            SwitchToNight();
        }
        else if (IsNight && timer >= nightDuration)
        {
            AchievementReporter.PlayTime(nightDuration);
            timer = 0f;
            SwitchToDay();
        }
    }

    private void SwitchToDay()
    {
        currentTime = TimeState.Day;
        OnDayStart?.Invoke();
    }

    private void SwitchToNight()
    {
        currentTime = TimeState.Night;
        OnNightStart?.Invoke();
    }

    public void ForceSetTime(TimeState newTime)
    {
        if (currentTime == newTime) return;

        currentTime = newTime;
        timer = 0f;

        hasRestoredTimeState = true;

        if (IsDay) OnDayStart?.Invoke();
        else OnNightStart?.Invoke();
    }

    public void SetTime(float savedTimer, bool isDay)
    {
        timer = Mathf.Max(0, savedTimer);
        currentTime = isDay ? TimeState.Day : TimeState.Night;

        hasRestoredTimeState = true;

        if (IsDay) OnDayStart?.Invoke();
        else OnNightStart?.Invoke();
    }
}
