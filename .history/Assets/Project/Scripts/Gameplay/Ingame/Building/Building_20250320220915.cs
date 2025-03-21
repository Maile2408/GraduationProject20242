using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

[Serializable]
public class Building : BuildingBase
{
    public List<Resource> Resources { get; private set; }

    public Building(string buildingID, string typeID, string name, int level, string state, Vector3 position)
        : base(buildingID, typeID, name, level, state, position)
    {
        this.Resources = new List<Resource>();
    }

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

    public static Building FromJson(string json)
    {
        return JsonConvert.DeserializeObject<Building>(json);
    }
}
