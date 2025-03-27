using UnityEngine;
using System;

public class TimeManager : SaiBehaviour
{
    public static TimeManager Instance { get; private set; }

    public enum TimeState { Day, Night }
    [SerializeField] TimeState currentTime = TimeState.Day;
    public TimeState CurrentTime => currentTime;

    public bool IsDay => currentTime == TimeState.Day;
    public bool IsNight => currentTime == TimeState.Night;

    [Header("Time Config")]
    [SerializeField] private float timePerPhase = 300f;
    [SerializeField] float timer;

    public static event Action OnDayStart;
    public static event Action OnNightStart;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    protected override void Update()
    {
        base.Update();

        timer += Time.deltaTime;
        if (timer >= timePerPhase)
        {
            timer = 0f;
            ToggleTime();
        }
    }

    public void ToggleTime()
    {
        currentTime = (currentTime == TimeState.Day) ? TimeState.Night : TimeState.Day;
        Debug.Log($"[TimeManager] Switched to: {currentTime}");

        if (IsDay) OnDayStart?.Invoke();
        else OnNightStart?.Invoke();
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
