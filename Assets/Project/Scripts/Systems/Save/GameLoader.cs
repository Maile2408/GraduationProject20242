using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLoader : MonoBehaviour
{
    public static GameLoader Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadAllGameData());
    }

    public IEnumerator LoadAllGameData()
    {
        yield return LoadCurrency();
        yield return LoadCityLevelAndTime();
        yield return LoadBuildings();
        yield return LoadWorkers();
        yield return LoadTrees();
        yield return LoadConstructions();
    }

    IEnumerator LoadCurrency()
    {
        var city = SaveManager.Instance.CurrentData.city;
        CurrencyManager.Instance.Coin = city.coin;
        CurrencyManager.Instance.NotifyCoinChanged();
        yield return null;
    }

    IEnumerator LoadCityLevelAndTime()
    {
        var city = SaveManager.Instance.CurrentData.city;
        CityLevelManager.Instance.SetLevelAndXP(city.cityLevel, city.xp);
        TimeManager.Instance.SetTime(city.timeCounter, city.timeState == "Day");
        yield return null;
    }

    IEnumerator LoadBuildings()
    {
        var city = SaveManager.Instance.CurrentData.city;
        LoadObjects<BuildingCtrl, BuildingSaveData>(city.buildings, IDType.Building);
        yield return null;
    }

    IEnumerator LoadWorkers()
    {
        var city = SaveManager.Instance.CurrentData.city;
        LoadObjects<WorkerCtrl, WorkerSaveData>(city.workers, IDType.Worker);
        yield return null;
    }

    IEnumerator LoadTrees()
    {
        var city = SaveManager.Instance.CurrentData.city;
        LoadObjects<TreeCtrl, TreeSaveData>(city.trees, IDType.Tree);
        yield return null;
    }

    IEnumerator LoadConstructions()
    {
        var city = SaveManager.Instance.CurrentData.city;
        LoadObjects<ConstructionCtrl, ConstructionSaveData>(city.constructions, IDType.Construction);
        yield return null;
    }

    private void LoadObjects<T, TData>(List<TData> savedList, IDType type)
        where T : MonoBehaviour, ISaveable<TData>
        where TData : class
    {
        foreach (var data in savedList)
        {
            string id = GetField<string>(data, "id");
            Vector3 position = GetField<Vector3>(data, "position");
            Quaternion rotation = GetField<Quaternion>(data, "rotation");

            T existing = FindById<T>(id);
            if (existing != null)
            {
                existing.LoadFromSave(data);
                continue;
            }

            string prefabName = GetField<string>(data, "type");
            string prefabPath = GetPrefabPathFromType(type, prefabName);

            GameObject go = PoolManager.Instance.Spawn(prefabPath);
            if (go == null) continue;

            go.transform.SetPositionAndRotation(position, rotation);

            var identifiable = go.GetComponent<Identifiable>();
            if (identifiable != null)
                identifiable.SetID(id);

            var ctrl = go.GetComponent<T>();
            ctrl?.LoadFromSave(data);

            RegisterToManager(ctrl);
        }
    }

    private string GetPrefabPathFromType(IDType type, string prefabName)
    {
        return type switch
        {
            IDType.Worker => PoolPrefabPath.Worker(prefabName),
            IDType.Tree => PoolPrefabPath.Tree(prefabName),
            IDType.Construction => PoolPrefabPath.Building(prefabName),
            IDType.Building => PoolPrefabPath.Building(prefabName),
            _ => string.Empty
        };
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
            if (ident != null && ident.ID == id)
                return t;
        }
        return null;
    }

    private U GetField<U>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName);
        return field != null ? (U)field.GetValue(obj) : default;
    }
}
