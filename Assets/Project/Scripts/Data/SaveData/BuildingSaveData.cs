using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuildingSaveData
{
    public string id;
    public string type;
    public int buildingInfoID;
    public Vector3 position;
    public Quaternion rotation;

    public List<Resource> inventory = new();

    public bool taxReady;
    public float taxTimer;
}
