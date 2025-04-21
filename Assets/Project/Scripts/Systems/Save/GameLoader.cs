using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;
    }

    void Start()
    {
        var city = SaveManager.Instance.CurrentData.city;

        // Load state for City
        //CurrencyManager.Instance.SetCoin(city.coin);
        //CityLevelManager.Instance.SetXP(city.xp);
        //CityLevelManager.Instance.SetLevel(city.cityLevel);
        //TimeManager.Instance.SetTime(city.timeCounter, city.timeState == "Day");

        // Load all world objects
        LoadObjects<BuildingCtrl, BuildingSaveData>(city.buildings, PoolPrefabPath.Building);
        LoadObjects<WorkerCtrl, WorkerSaveData>(city.workers, PoolPrefabPath.Worker);
        LoadObjects<TreeCtrl, TreeSaveData>(city.trees, PoolPrefabPath.Tree);
        LoadObjects<ConstructionCtrl, ConstructionSaveData>(city.constructions, PoolPrefabPath.Building);

        // Restore unlocks
        //UnlockManager.Instance.RestoreUnlocks(city.unlockedBuildingIDs);
        //AchievementManager.Instance.RestoreUnlocks(city.unlockedAchievements);
    }

    private void LoadObjects<T, TData>(List<TData> savedList, Func<string, string> pathFunc)
        where T : MonoBehaviour, ISaveable<TData>
        where TData : class
    {
        foreach (var data in savedList)
        {
            string id = GetField<string>(data, "id");
            Vector3 position = GetField<Vector3>(data, "position");
            Quaternion rotation = HasField(data, "rotation") ? GetField<Quaternion>(data, "rotation") : Quaternion.identity;
            string type = GetField<string>(data, "type");

            T existing = FindById<T>(id);
            if (existing != null)
            {
                existing.LoadFromSave(data);
                continue;
            }

            string prefabPath = pathFunc(type);
            GameObject go = PoolManager.Instance.Spawn(prefabPath);
            if (go == null) continue;

            go.transform.SetPositionAndRotation(position, rotation);

            var iden = go.GetComponent<Identifiable>();
            if (iden != null) iden.id = id;

            var ctrl = go.GetComponent<T>();
            ctrl?.LoadFromSave(data);

            RegisterToManager(ctrl);
        }
    }

    private void RegisterToManager<T>(T ctrl)
    {
        switch (ctrl)
        {
            case BuildingCtrl b: BuildingManager.Instance?.AddBuilding(b); break;
            case WorkerCtrl w: WorkerManager.Instance?.AddWorker(w); break;
            case TreeCtrl t: TreeManager.Instance?.TreeAdd(t); break;
            case ConstructionCtrl c:
                if (c.TryGetComponent<AbstractConstruction>(out var abstractC))
                    ConstructionManager.Instance?.AddConstruction(abstractC);
                break;
        }
    }

    private T FindById<T>(string id) where T : MonoBehaviour
    {
        foreach (var t in GameObject.FindObjectsOfType<T>())
        {
            var ident = t.GetComponent<Identifiable>();
            if (ident != null && ident.id == id)
                return t;
        }
        return null;
    }

    private U GetField<U>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName);
        return field != null ? (U)field.GetValue(obj) : default;
    }

    private bool HasField(object obj, string fieldName)
    {
        return obj.GetType().GetField(fieldName) != null;
    }
}
