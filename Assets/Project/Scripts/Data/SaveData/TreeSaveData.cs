using UnityEngine;
using System;

[Serializable]
public class TreeSaveData
{
    public string id;
    public string type;
    public Vector3 position;
    public Quaternion rotation;

    public int currentLevel;
    public float treeTimer;

    public float generatorTimer;
}
