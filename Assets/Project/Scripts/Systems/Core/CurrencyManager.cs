using UnityEngine;
using System;

public class CurrencyManager : SaiBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] float currentCoin = 2000;

    public static event Action OnCoinChanged;

    public float CurrentCoin() => currentCoin;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only 1 CurrencyManager allowed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        OnCoinChanged?.Invoke();

        //Achievement
        AchievementReporter.UpdateCurrentCoin(currentCoin);
    }

    public void AddCoin(float amount)
    {
        currentCoin += amount;
        OnCoinChanged?.Invoke();

        //Achievement
        AchievementReporter.EarnCoin(amount);
        AchievementReporter.UpdateCurrentCoin(currentCoin);
    }

    public void SpendCoin(float amount)
    {
        if (currentCoin >= amount)
        {
            currentCoin -= amount;
            OnCoinChanged?.Invoke();

            //Achievement
            AchievementReporter.SpendCoin(amount);
            AchievementReporter.UpdateCurrentCoin(currentCoin);
        }
        Debug.Log("Not enough money!");
    }
}
