using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building : BuildingBase
{
    public List<Resource> Resources { get; private set; }

    public Building(string buildingID, string typeID, string name, int level, string state, Vector3 position)
        : base(buildingID, typeID, name, level, state, position)
    {
        this.Resources = new List<Resource>();
    }

    public void AddResource(Resource resource)
    {
        Resource existing = Resources.Find(r => r.Name == resource.Name);
        if (existing != null) existing.Amount += resource.Amount;
        else Resources.Add(resource);
    }

    public override void PerformFunction()
    {
        Debug.Log($"{Name} is performing its function.");
    }
}
