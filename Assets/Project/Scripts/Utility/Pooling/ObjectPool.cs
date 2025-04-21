using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly GameObject prefab;
    private readonly Queue<GameObject> pool = new Queue<GameObject>();
    private readonly Transform poolParent;

    public ObjectPool(GameObject prefab, Transform poolParent)
    {
        this.prefab = prefab;
        this.poolParent = poolParent;
    }

    public GameObject Spawn(Transform parent = null)
    {
        GameObject obj = null;

        while (pool.Count > 0)
        {
            obj = pool.Dequeue();
            if (obj != null) break;
        }

        if (obj == null)
        {
            obj = GameObject.Instantiate(prefab);
        }

        if (obj == null)
        {
            Debug.LogError("[ObjectPool] Failed to spawn object (still null after instantiate)");
            return null;
        }

        obj.transform.SetParent(parent, false);
        obj.SetActive(true);

        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnSpawn();
        }

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        if (obj == null) return;

        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnDespawn();
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolParent, false);
        pool.Enqueue(obj);
    }
}