using UnityEngine;

public class WarehouseWH : Warehouse
{
    [SerializeField] float storageCapacity = 500f;

    public virtual float GetStorageCapacity() => storageCapacity;

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
