using System;
using System.Collections.Generic;

[Serializable]
public class CitySaveData
{
    public string cityName;
    public int cityLevel;
    public int xp;
    public float coin;

    public string timeState; // "Day" / "Night"
    public float timeCounter;

    public List<AchievementSaveData> achievements = new();

    public List<BuildingSaveData> buildings = new();
    public List<ConstructionSaveData> constructions = new();
    public List<WorkerSaveData> workers = new();
    public List<TreeSaveData> trees = new();

    public string lastSaveTime;
}
