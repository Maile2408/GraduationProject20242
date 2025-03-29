using System.Collections.Generic;
using UnityEngine;

public class HouseWH : Warehouse
{
    [Header("House Consumer")]
    [SerializeField] protected List<Resource> resConsume = new();
    public bool canConsume = true;

    [SerializeField] protected float consumeTimer = 0f;
    [SerializeField] protected float consumeDelay = 60f;

    protected Workers workers;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadWorkers();
        this.LoadResConsume();
    }

    protected virtual void LoadWorkers()
    {
        if (this.workers != null) return;
        this.workers = GetComponent<Workers>();
        Debug.Log(transform.name + " LoadWorkers", gameObject);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.CheckCanConsume();
        this.Consuming();
    }

    protected virtual void CheckCanConsume()
    {
        this.canConsume = TimeManager.Instance.IsNight;
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

    protected virtual void LoadResConsume()
    {
        Resource res = new Resource
        {
            name = ResourceName.water,
            number = 1
        };

        this.resConsume.Clear();
        this.resConsume.Add(res);
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
