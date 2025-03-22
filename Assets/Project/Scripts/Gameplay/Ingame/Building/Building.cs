using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Building
{
    public string buildingID;
    public string typeID;
    public int level;
    public string state;
    public Vector3 position;
    public List<Resource> storedResources;
}
