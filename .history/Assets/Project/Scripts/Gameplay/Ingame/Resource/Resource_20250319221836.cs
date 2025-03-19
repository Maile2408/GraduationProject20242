using System;

[Serializable]
public class Resource
{
    public string ResourceID { get; private set; }
    public ResourceName Name { get; private set; }
    public float Amount { get; private set; }

    public Resource(string resourceID, ResourceName name, float initialAmount = 0)
    {
        ResourceID = resourceID;
        Name = name;
        Amount = Math.Max(0, initialAmount);
    }

    public void AddAmount(float value)
    {
        if (value < 0) throw new ArgumentException("Cannot add a negative amount.");
        Amount += value;
    }

    public bool DeductAmount(float value)
    {
        if (value < 0) throw new ArgumentException("Cannot deduct a negative amount.");
        if (Amount >= value)
        {
            Amount -= value;
            return true;
        }
        return false;
    }
}
