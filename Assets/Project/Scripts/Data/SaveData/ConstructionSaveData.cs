using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class ConstructionSaveData
{
    public string id;
    public string type;                            
    public int buildingInfoID;                       
    public Vector3 position;
    public Quaternion rotation;

    public List<Resource> resourceProgress = new();
}
