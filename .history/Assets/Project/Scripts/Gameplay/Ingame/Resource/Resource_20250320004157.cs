using System;

[Serializable]
public class Resource
{
    public ResourceName Name { get; private set; }
    public float Amount { get; private set; }
}
