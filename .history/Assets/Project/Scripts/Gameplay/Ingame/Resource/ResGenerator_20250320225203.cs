using System.Collections.Generic;
using UnityEngine;

public class ResGenerator : Warehouse
{
    [Header("Res Generator")]
    [SerializeField] protected List<Resource> resCreate;
    [SerializeField] protected List<Resource> resRequire;
    [SerializeField] protected float createTimer = 0f;
    [SerializeField] protected float createDelay = 7f;
    public bool canCreate = true;

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.GenerateResources();
    }

    protected virtual void GenerateResources()
    {
        if (!this.canCreate) return; 
        if (!this.HasRequiredResources()) return; 

        this.createTimer += Time.fixedDeltaTime;
        if (this.createTimer < this.createDelay) return;

        this.createTimer = 0;
        this.ConsumeRequiredResources();
        this.ProduceResources();
    }

    protected virtual bool HasRequiredResources()
    {
        if (this.resRequire.Count == 0) return true; 

        foreach (Resource required in this.resRequire)
        {
            ResHolder resHolder = this.GetResource(required.name);
            if (resHolder == null || resHolder.Current() < required.amount) return false; 
        }

        return true;
    }

    protected virtual void ConsumeRequiredResources()
    {
        foreach (Resource required in this.resRequire)
        {
            ResHolder resHolder = this.GetResource(required.name);
            if (resHolder != null) resHolder.Deduct(required.amount); 
        }
    }

    protected virtual void ProduceResources()
    {
        foreach (Resource produced in this.resCreate)
        {
            ResHolder resHolder = this.GetResource(produced.name);
            if (resHolder != null) resHolder.Add(produced.amount);
        }
    }

    public virtual float GetCreateDelay() => this.createDelay;

    public virtual bool IsAllResMax()
    {
        foreach (ResHolder resHolder in this.resHolders)
        {
            if (!resHolder.IsMax()) return false;
        }
        return true;
    }

    public virtual List<Resource> TakeAll()
    {
        List<Resource> resources = new List<Resource>();
        foreach (ResHolder resHolder in this.resHolders)
        {
            Resource newResource = new Resource
            {
                name = resHolder.Name(),
                amount = resHolder.TakeAll()
            };
        }
        return resources;
    }
}
