using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CitySaveData
{
    public string cityName;
    public int cityLevel;
    public int xp;
    public int coin;

    public string timeState; // "Day" / "Night"
    public float timeCounter;

    public List<int> unlockedBuildingIDs = new();
    public List<string> unlockedAchievements = new();

    public List<BuildingSaveData> buildings = new();
    public List<ConstructionSaveData> constructions = new();
    public List<WorkerSaveData> workers = new();
    public List<TreeSaveData> trees = new();

    public string lastSaveTime;
}
