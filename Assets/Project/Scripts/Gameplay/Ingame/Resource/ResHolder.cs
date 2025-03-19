using System;
using UnityEngine;

public class ResHolder : SaiBehaviour
{
    [SerializeField] protected ResourceName resourceName;
    [SerializeField] private float resCurrent = 0;
    [SerializeField] private float resMax = Mathf.Infinity;

    public ResourceName ResourceType => resourceName;
    public float CurrentAmount => resCurrent;
    public float MaxAmount => resMax;

    public ResHolder(ResourceName resource, float maxCapacity)
    {
        this.resourceName = resource;
        this.resMax = Mathf.Max(0, maxCapacity);
        this.resCurrent = 0;
    }

    public float Add(float amount)
    {
        if (amount < 0) return 0;
        float availableSpace = resMax - resCurrent;
        float addAmount = Mathf.Min(amount, availableSpace);
        resCurrent += addAmount;
        return addAmount;
    }

    public float Deduct(float amount)
    {
        if (amount < 0) return 0;
        float deductAmount = Mathf.Min(amount, resCurrent);
        resCurrent -= deductAmount;
        return deductAmount;
    }

    public bool IsFull() => resCurrent >= resMax;
    public bool IsEmpty() => resCurrent <= 0;
}
