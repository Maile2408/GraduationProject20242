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
}
