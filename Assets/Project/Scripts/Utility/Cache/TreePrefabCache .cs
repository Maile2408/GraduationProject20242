using UnityEngine;

public class TreePrefabCache : MonoBehaviour
{
    private static GameObject[] treePrefabs;

    public static GameObject GetRandomTree()
    {
        if (treePrefabs == null || treePrefabs.Length == 0)
            LoadTreePrefabs();

        if (treePrefabs == null || treePrefabs.Length == 0)
        {
            Debug.LogError("WorkerPrefabCache: No worker prefabs found!");
            return null;
        }

        int rand = Random.Range(0, treePrefabs.Length);
        return treePrefabs[rand];
    }

    private static void LoadTreePrefabs()
    {
        treePrefabs = Resources.LoadAll<GameObject>(PoolPrefabPath.Tree(""));
    }
}