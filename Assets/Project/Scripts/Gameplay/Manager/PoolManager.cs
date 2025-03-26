using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    private Dictionary<string, ObjectPool> pools = new();
    private Dictionary<GameObject, ObjectPool> objectToPoolMap = new();

    [Header("Pool Config")] 
    [SerializeField] private Transform poolRoot;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (poolRoot == null)
        {
            GameObject root = new("__Pools");
            poolRoot = root.transform;
        }
    }
    
    public GameObject Spawn(string prefabPath, Transform parent = null)
    {
        if (!pools.ContainsKey(prefabPath))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError($"[PoolManager] Prefab not found in Resources: {prefabPath}");
                return null;
            }
            pools[prefabPath] = new ObjectPool(prefab, poolRoot);
        }

        GameObject obj = pools[prefabPath].Spawn(parent);
        objectToPoolMap[obj] = pools[prefabPath];
        return obj;
    }

    public void Despawn(GameObject obj)
    {
        if (objectToPoolMap.TryGetValue(obj, out var pool))
        {
            pool.Despawn(obj);
        }
        else
        {
            Debug.LogWarning("[PoolManager] Trying to despawn object that was not spawned by PoolManager. Disabling instead.");
            obj.SetActive(false);
            obj.transform.SetParent(poolRoot);
        }
    }
}