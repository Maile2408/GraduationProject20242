using UnityEngine;
using System;

[Serializable]
public class TreeSaveData
{
    public string id;
    public int treeType;
    public Vector3 position;
    public Quaternion rotation;

    public int currentLevel;
    public float treeTimer;
    public bool isMaxLevel;

    public float generatorTimer;
}
