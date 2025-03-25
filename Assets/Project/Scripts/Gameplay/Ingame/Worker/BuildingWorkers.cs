using System.Collections.Generic;
using UnityEngine;

public class BuildingWorkers : Workers
{
   [SerializeField] private BuildingInfo buildingInfo;

   protected override void LoadComponents()
   {
      base.LoadComponents();

      this.LoadBuildingInfo();
      this.LoadMaxWorker();
   }

   protected virtual void LoadBuildingInfo()
   {
      if (this.buildingInfo != null) return;

      this.buildingInfo = GetComponent<BuildingInfo>();
      if (this.buildingInfo == null)
      {
         Debug.LogError($"{transform.name}: Missing BuildingInfo");
      }
   }

   public virtual void LoadMaxWorker()
   {
      if (buildingInfo?.CurrentLevelData?.workerCapacity == null) return;
      this.maxWorker = buildingInfo.CurrentLevelData.workerCapacity.Value;
   }


   public virtual void SpawnInitialWorkers(string folder, Transform door)
   {
      if (buildingInfo == null || buildingInfo.CurrentLevelData == null)
      {
         Debug.LogWarning($"{transform.name}: BuildingInfo is not ready!");
         return;
      }

      int capacity = maxWorker;

      GameObject[] prefabs = Resources.LoadAll<GameObject>(folder);
      if (prefabs == null || prefabs.Length == 0)
      {
         Debug.LogWarning("Worker prefab is not found in Resources/" + folder);
         return;
      }

      for (int i = 0; i < capacity; i++)
      {
         GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
         string path = PoolPrefabPath.Worker(prefab.name);

         GameObject obj = PoolManager.Instance.Spawn(path);
         if (obj == null) continue;

         obj.transform.position = door.position;

         WorkerCtrl worker = obj.GetComponent<WorkerCtrl>();
         if (worker == null) continue;

         worker.workerBuildings.AssignHome(GetComponent<BuildingCtrl>());

         this.AddWorker(worker);
      }
   }


   public virtual void DespawnAllWorkers()
   {
      foreach (var worker in workers)
      {
         if (worker == null) continue;
         PoolManager.Instance.Despawn(worker.gameObject);
      }

      workers.Clear();
   }
}
