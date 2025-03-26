using System.Collections.Generic;
using UnityEngine;

public class WarehouseBuilding : Warehouse
{
    public override bool IsFull()
    {
        foreach (ResHolder resHolder in this.resHolders)
        {
            if (!resHolder.IsMax()) return false;
        }

        return true;
    }
}
