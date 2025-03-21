using System;
using UnityEngine;

[Serializable]
public abstract class Building
{
    public string buildingID;
    public string typeID;
    public string name;
    public int level;
    public string state;
    public Vector3 position;
}
