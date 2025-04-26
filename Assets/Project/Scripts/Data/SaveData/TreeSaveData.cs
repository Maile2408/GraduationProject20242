using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class TreeSaveData
{
    public string id;
    public string type;
    public Vector3 position;
    public Quaternion rotation;

    public bool isMaxLevel;
    public int currentLevel;
    public float treeTimer;

    public List<Resource> inventory = new();
    public float generatorTimer;
}
