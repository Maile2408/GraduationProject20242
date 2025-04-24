using System;
using System.Linq;
using UnityEngine;

public class SaveStateCollector : MonoBehaviour
{
    public static SaveStateCollector Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    public void SaveAll()
    {
        var city = SaveManager.Instance.CurrentData.city;

        city.lastSaveTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        city.coin = CurrencyManager.Instance.Coin;
        city.xp = CityLevelManager.Instance.XP;
        city.cityLevel = CityLevelManager.Instance.Level;

        city.timeCounter = TimeManager.Instance.TimeCounter;
        city.timeState = TimeManager.Instance.CurrentTime.ToString();

        city.achievements = AchievementManager.Instance.GetAllSaveData();

        // BUILDINGS
        city.buildings.Clear();
        foreach (var b in BuildingManager.Instance.BuildingCtrls())
            city.buildings.Add(b.Save());

        // WORKERS
        city.workers.Clear();
        foreach (var w in WorkerManager.Instance.WorkerCtrls())
            city.workers.Add(w.Save());

        // TREES
        city.trees.Clear();
        foreach (var t in TreeManager.Instance.Trees())
            city.trees.Add(t.Save());

        // CONSTRUCTIONS
        city.constructions.Clear();
        foreach (var construction in ConstructionManager.Instance.GetAll())
        {
            var c = construction.GetComponent<ConstructionCtrl>();
            if (c != null)
                city.constructions.Add(c.Save());
        }

        SaveManager.Instance.SaveAndUpload();
    }
}
