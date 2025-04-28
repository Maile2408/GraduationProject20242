using UnityEngine;
using System;

public class CurrencyManager : SaiBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] float currentCoin = 2000;

    public static event Action OnCoinChanged;

    public float Coin
    {
        get => currentCoin;
        set
        {
            currentCoin = Mathf.Max(0, value);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();

        //Achievement
        AchievementReporter.UpdateCurrentCoin(currentCoin);
    }

    public void NotifyCoinChanged()
    {
        OnCoinChanged?.Invoke();
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
        //Debug.Log("Not enough money!");
    }
}
