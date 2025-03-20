using System;

[Serializable]
public class Resource
{
    public ResourceName Name { get; private set; }
    public float Amount { get; set; }

    public Resource(ResourceName name, float amount)
    {
        this.Name = name;
        this.Amount = amount;
    }
}
