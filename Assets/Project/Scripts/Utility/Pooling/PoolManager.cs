using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SaiBehaviour
{
    public static PoolManager Instance;

    [SerializeField] private Transform poolParent;

    private readonly Dictionary<string, ObjectPool> poolByPath = new Dictionary<string, ObjectPool>();
    private readonly Dictionary<GameObject, ObjectPool> poolByObject = new Dictionary<GameObject, ObjectPool>();

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public GameObject Spawn(string prefabPath, Transform parent = null)
    {
        if (!poolByPath.TryGetValue(prefabPath, out var pool))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError("Prefab not found at: " + prefabPath);
                return null;
            }

            pool = new ObjectPool(prefab, poolParent);
            poolByPath.Add(prefabPath, pool);
        }

        GameObject obj = pool.Spawn(parent);
        if (obj != null)
        {
            poolByObject[obj] = pool;
        }

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        if (obj == null) return;

        if (poolByObject.TryGetValue(obj, out var pool))
        {
            pool.Despawn(obj);
            poolByObject.Remove(obj);
        }
        else
        {
            Debug.LogWarning("[PoolManager] Object not from pool: " + obj.name);
            GameObject.Destroy(obj);
        }
    }

    public bool HasSpawned(GameObject obj)
    {
        return poolByObject.ContainsKey(obj);
    }

}
