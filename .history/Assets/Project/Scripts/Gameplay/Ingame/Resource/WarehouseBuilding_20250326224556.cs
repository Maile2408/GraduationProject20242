using System.Collections.Generic;
using UnityEngine;

public class WarehouseBuilding : Warehouse
{
    [SerializeField] float storageCapacity = 500;
    public override bool IsFull()
    {
        foreach (ResHolder resHolder in this.resHolders)
        {
            if (!resHolder.IsMax()) return false;
        }

        return true;
    }
}
