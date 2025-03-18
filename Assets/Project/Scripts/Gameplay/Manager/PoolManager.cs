using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SaiBehaviour
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    public static PoolManager Instance { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        if (Instance == null) Instance = this;
        else Debug.LogWarning("PoolManager đã tồn tại.");
    }

    public void CreatePool(string key, GameObject prefab, int initialSize)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            poolDictionary[key] = new Queue<GameObject>();

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                poolDictionary[key].Enqueue(obj);
            }
        }
    }

    public GameObject GetFromPool(string key, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Pool '{key}' chưa được tạo. Hãy gọi CreatePool trước.");
            return null;
        }

        GameObject obj;
        if (poolDictionary[key].Count > 0)
        {
            obj = poolDictionary[key].Dequeue();
        }
        else
        {
            Debug.LogWarning($"Pool '{key}' hết object, tạo mới.");
            return null;
        }

        obj.transform.position = position;
        obj.transform.rotation = rotation;
        obj.SetActive(true);

        // Nếu object có interface IPoolable, gọi OnSpawn()
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnSpawn();

        return obj;
    }

    public void ReturnToPool(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Pool '{key}' không tồn tại. Hủy object này.");
            Destroy(obj);
            return;
        }

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);

        // Nếu object có interface IPoolable, gọi OnDespawn()
        IPoolable poolable = obj.GetComponent<IPoolable>();
        poolable?.OnDespawn();
    }
}
