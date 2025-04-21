using UnityEngine;

public class WorkerPrefabCache : MonoBehaviour
{
    private static GameObject[] workerPrefabs;

    public static GameObject GetRandomWorker()
    {
        if (workerPrefabs == null || workerPrefabs.Length == 0)
            LoadWorkerPrefabs();

        if (workerPrefabs == null || workerPrefabs.Length == 0)
        {
            Debug.LogError("WorkerPrefabCache: No worker prefabs found!");
            return null;
        }

        int rand = Random.Range(0, workerPrefabs.Length);
        return workerPrefabs[rand];
    }

    private static void LoadWorkerPrefabs()
    {
        workerPrefabs = Resources.LoadAll<GameObject>(PoolPrefabPath.Worker(""));
    }
}