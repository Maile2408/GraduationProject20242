using System;
using UnityEngine;

[Serializable]
public abstract class BuildingBase
{
    public string BuildingID;
    public string TypeID;
    public string Name;
    public int Level;
    public string State;
    public Vector3 Position;

    public BuildingBase(string buildingID, string typeID, string name, int level, string state, Vector3 position)
    {
        this.BuildingID = buildingID;
        this.TypeID = typeID;
        this.Name = name;
        this.Level = level;
        this.State = state;
        this.Position = position;
    }
}
