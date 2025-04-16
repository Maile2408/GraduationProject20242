using System;
using UnityEngine;

public class WarehouseWH : Warehouse
{
    [SerializeField] float storageCapacity = 500f;

    public static event Action OnStorageChanged;

    public virtual float GetStorageCapacity() => storageCapacity;

    protected override void Start()
    {
        StorageResourceManager.Instance.ReportStorageAchievements();
    }

    public override ResHolder AddResource(ResourceName resourceName, float number)
    {
        ResHolder res = this.GetResource(resourceName);
        res.Add(number);

        OnStorageChanged?.Invoke();
        StorageResourceManager.Instance.ReportStorageAchievements();
        return res;
    }

    public override ResHolder RemoveResource(ResourceName resourceName, float number)
    {
        ResHolder res = this.GetResource(resourceName);
        if (res.Current() < number) return null;
        res.Deduct(number);

        OnStorageChanged?.Invoke();
        StorageResourceManager.Instance.ReportStorageAchievements();
        return res;
    }

    public override bool IsFull()
    {
        float totalStored = 0f;
        foreach (ResHolder resHolder in this.resHolders)
        {
            totalStored += resHolder.Current();
        }

        return totalStored >= storageCapacity;
    }
}
