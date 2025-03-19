using System;

[Serializable]
public class Resource
{
    public string ResourceID { get; private set; }
    public ResourceName Name { get; private set; }
    public float Amount { get; private set; }
}
