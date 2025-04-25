using System;
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
        {
            if (b == null) continue;
            try { city.buildings.Add(b.Save()); }
            catch (Exception e) { Debug.LogWarning("Save Building failed: " + e.Message); }
        }

        // WORKERS
        city.workers.Clear();
        foreach (var w in WorkerManager.Instance.WorkerCtrls())
        {
            if (w == null) continue;
            try { city.workers.Add(w.Save()); }
            catch (Exception e) { Debug.LogWarning("Save Worker failed: " + e.Message); }
        }

        // TREES
        city.trees.Clear();
        foreach (var t in TreeManager.Instance.Trees())
        {
            if (t == null) continue;
            try { city.trees.Add(t.Save()); }
            catch (Exception e) { Debug.LogWarning("Save Tree failed: " + e.Message); }
        }

        // CONSTRUCTIONS
        city.constructions.Clear();
        foreach (var construction in ConstructionManager.Instance.GetAll())
        {
            if (construction == null) continue;

            var c = construction.GetComponent<ConstructionCtrl>();
            if (c != null)
            {
                try { city.constructions.Add(c.Save()); }
                catch (Exception e) { Debug.LogWarning("Save Construction failed: " + e.Message); }
            }
        }

        SaveManager.Instance.SaveAndUpload();
    }
}
