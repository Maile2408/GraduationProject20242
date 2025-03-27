using UnityEngine;

public class HouseCtrl : BuildingCtrl
{
    protected override void Start()
    {
        base.Start();
        for (int i = 0; i < this.workers.MaxWorker(); i++)
        {
            this.SpawnWorker();
        }
    }

    protected virtual void SpawnWorker()
    {
        GameObject prefab = WorkerPrefabCache.GetRandomWorker();

        string path = PoolPrefabPath.Worker(prefab.name);
        GameObject workerObj = PoolManager.Instance.Spawn(path);
    
        Vector2 randomOffset = Random.insideUnitCircle * 2f;
        Vector3 spawnPos = this.door.position + new Vector3(randomOffset.x, 0, randomOffset.y);

        workerObj.transform.position = spawnPos;
        workerObj.transform.rotation = this.door.rotation;

        WorkerCtrl worker = workerObj.GetComponent<WorkerCtrl>();

        WorkerManager.instance.AddWorker(worker);
    }
}