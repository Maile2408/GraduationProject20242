using System;
using UnityEngine;

[Serializable]
public class ResHolder
{
    private ResourceName resourceName;
    private float resCurrent;
    private float resMax;

    public ResourceName ResourceType => resourceName;
    public float CurrentAmount => resCurrent;
    public float MaxAmount => resMax;

    public ResHolder(ResourceName resource, float maxCapacity)
    {
        this.resourceName = resource;
        this.resCurrent = 0;
        this.resMax = Mathf.Max(0, maxCapacity);
    }

    public float Add(float amount)
    {
        if (amount < 0) throw new ArgumentException("Cannot add a negative amount.");
        
        float availableSpace = resMax - resCurrent;
        float addAmount = Mathf.Min(amount, availableSpace);
        resCurrent += addAmount;
        
        return addAmount; 
    }

    public float Deduct(float amount)
    {
        if (amount < 0) throw new ArgumentException("Cannot deduct a negative amount.");

        float deductAmount = Mathf.Min(amount, resCurrent);
        resCurrent -= deductAmount;

        return deductAmount; 
    }

    public float TakeAll()
    {
        float taken = resCurrent;
        resCurrent = 0;
        return taken;
    }

    public void SetLimit(float max)
    {
        resMax = Mathf.Max(0, max); // Không cho phép đặt max dưới 0
    }

    public bool IsFull()
    {
        return resCurrent >= resMax;
    }

    public bool IsEmpty()
    {
        return resCurrent <= 0;
    }
}
