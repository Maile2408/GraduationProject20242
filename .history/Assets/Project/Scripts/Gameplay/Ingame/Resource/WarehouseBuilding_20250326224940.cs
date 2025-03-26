using System.Collections.Generic;
using UnityEngine;

public class WarehouseBuilding : Warehouse
{
    [SerializeField] float storageCapacity = 500f;

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
