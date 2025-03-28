using System.Collections.Generic;
using UnityEngine;

public class HouseWH : Warehouse
{
    [Header("House Consumer")]
    [SerializeField] protected List<Resource> resConsume = new();
    public bool canConsume = true;

    [SerializeField] protected float consumeTimer = 0f;
    [SerializeField] protected float consumeDelay = 15f;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.Consuming();
    }

    protected virtual void Consuming()
    {
        if (!this.canConsume) return;

        this.consumeTimer += Time.fixedDeltaTime;
        if (this.consumeTimer < this.consumeDelay) return;
        this.consumeTimer = 0f;

        foreach (Resource res in this.resConsume)
        {
            ResHolder holder = this.GetResource(res.name);
            if (holder == null) continue;

            if (holder.Current() >= res.number) holder.Deduct(res.number);
        }
    }

    public override List<Resource> NeedResoures()
    {
        List<Resource> needs = new List<Resource>();

        foreach (Resource res in this.resConsume)
        {
            ResHolder holder = this.GetResource(res.name);
            if (holder == null) continue;

            float missing = holder.Max() - holder.Current();
            if (missing > 0)
            {
                needs.Add(new Resource
                {
                    name = res.name,
                    number = missing
                });
            }
        }

        return needs;
    }
}
