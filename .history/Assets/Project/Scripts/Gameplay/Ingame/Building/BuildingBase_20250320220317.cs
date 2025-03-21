using System;
using UnityEngine;

[Serializable]
public abstract class BuildingBase
{
    public string BuildingID { get; protected set; }
    public string TypeID { get; protected set; }
    public string Name { get; protected set; }
    public int Level { get; protected set; }
    public string State { get; protected set; }
    public Vector3 Position { get; protected set; }

    public BuildingBase(string buildingID, string typeID, string name, int level, string state, Vector3 position)
    {
        this.BuildingID = buildingID;
        this.TypeID = typeID;
        this.Name = name;
        this.Level = level;
        this.State = state;
        this.Position = position;
    }

    public virtual void Upgrade()
    {
        Level++;
        Debug.Log($"{Name} upgraded to level {Level}");
    }

    public void ChangeState(string newState)
    {
        this.State = newState;
        Debug.Log($"{Name} state changed to {State}");
    }
}
