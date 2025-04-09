using UnityEngine;
using System;

public class CurrencyManager : SaiBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] float currentCoin = 1000;

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
    }

    public void AddCoin(float amount)
    {
        currentCoin += amount;
        OnCoinChanged?.Invoke();
    }

    public bool SpendCoin(float amount)
    {
        if (currentCoin >= amount)
        {
            currentCoin -= amount;
            OnCoinChanged?.Invoke();
            return true;
        }

        Debug.Log("Not enough money!");
        return false;
    }
}
