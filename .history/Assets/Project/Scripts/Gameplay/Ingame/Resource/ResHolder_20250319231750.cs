using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResHolder : SaiBehaviour
{
    [Header("Res Holder")]
    [SerializeField] protected ResourceName resourceName;
    [SerializeField] private float resCurrent = 0;
    [SerializeField] private float resMax = Mathf.Infinity;

    public ResourceName ResourceType => resourceName;
    public float CurrentAmount => resCurrent;
    public float MaxAmount => resMax;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadResName();
    }

    protected virtual void LoadResName()
    {
        if (this.resourceName != ResourceName.noResource) return;

        string name = transform.name;
        this.resourceName = ResNameParser.FromString(name);
        Debug.Log(transform.name + ": LoadResName");
    }

    public virtual float Add(float amount)
    {
        if (amount < 0) return 0;

        float availableSpace = resMax - resCurrent;
        float addAmount = Mathf.Min(amount, availableSpace);
        resCurrent += addAmount;

        return addAmount;
    }

    public virtual float Deduct(float amount)
    {
        if (amount < 0) return 0;

        float deductAmount = Mathf.Min(amount, resCurrent);
        resCurrent -= deductAmount;

        return deductAmount;
    }

    public virtual float TakeAll()
    {
        float taken = resCurrent;
        resCurrent = 0;
        return taken;
    }

    public virtual void SetLimit(float max)
    {
        resMax = Mathf.Max(0, max);
    }

    public virtual bool IsMax()
    {
        return resCurrent >= resMax;
    }

    public virtual bool IsEmpty()
    {
        return resCurrent <= 0;
    }
}
