using UnityEngine;

public class ResHolder : SaiBehaviour
{
    [Header("Res Holder")]
    [SerializeField] protected ResourceName resourceName = ResourceName.noResource;
    [SerializeField] private float resCurrent = 0; 
    [SerializeField] private float resMax = Mathf.Infinity; 

    public float Current() => resCurrent; 
    public float Max() => resMax; 

    protected override void LoadComponents()
    {
        this.LoadResName();
    }

    protected virtual void LoadResName()
    {
        if (this.resourceName != ResourceName.noResource) return;

        string name = transform.name;
        this.resourceName = ResNameParser.FromString(name);
        Debug.Log(transform.name + ": LoadResName");
    }

    public virtual ResourceName Name()
    {
        return this.resourceName;
    }

    public virtual float Add(float number)
    {
        if (number <= 0) return resCurrent; 

        resCurrent = Mathf.Clamp(resCurrent + number, 0, resMax); 
        return resCurrent;
    }

    public virtual float Deduct(float number)
    {
        if (number <= 0) return resCurrent; 

        resCurrent = Mathf.Max(resCurrent - number, 0); 
        return resCurrent;
    }

    public virtual float TakeAll()
    {
        float take = resCurrent;
        resCurrent = 0;
        return take;
    }

    public virtual void SetLimit(float max)
    {
        if (max <= 0) return; 

        resMax = max;
        resCurrent = Mathf.Min(resCurrent, resMax); 
    }

    public virtual bool IsMax()
    {
        return resCurrent >= resMax;
    }
}
