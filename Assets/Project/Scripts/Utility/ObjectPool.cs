using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> pool = new();
    private Transform poolParent;

    public ObjectPool(GameObject prefab, Transform poolParent)
    {
        this.prefab = prefab;
        this.poolParent = poolParent;
    }

    public GameObject Spawn(Transform parent = null)
    {
        GameObject obj = pool.Count > 0 ? pool.Dequeue() : GameObject.Instantiate(prefab);
        obj.transform.SetParent(parent);
        obj.SetActive(true);

        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnSpawn();
        }

        return obj;
    }

    public void Despawn(GameObject obj)
    {
        if (obj.TryGetComponent<IPoolable>(out var poolable))
        {
            poolable.OnDespawn();
        }

        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
        pool.Enqueue(obj);
    }
}
