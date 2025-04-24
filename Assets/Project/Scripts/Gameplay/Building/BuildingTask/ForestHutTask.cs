using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForestHutTask : BuildingTask
{
    [Header("Forest Hut")]
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected GameObject plantTreeObj;
    [SerializeField] protected int treeMax = 7;
    [SerializeField] protected float treeRange = 20f;
    [SerializeField] protected float treeDistance = 7f;
    [SerializeField] protected float treeRemoveSpeed = 16f;
    [SerializeField] protected List<TreeCtrl> trees = new();

    protected override void Start()
    {
        base.Start();
        this.LoadNearByTrees();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        this.RemoveDeadTrees();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadObjects();
        this.LoadGroundLayer();
    }

    protected virtual void LoadGroundLayer()
    {
        if (this.groundLayer.value != 0) return;
        this.groundLayer = LayerMask.GetMask("Ground");
    }

    protected virtual void LoadObjects()
    {
        if (this.plantTreeObj != null) return;
        this.plantTreeObj = Resources.Load<GameObject>(PoolPrefabPath.Point("MaskPositionObject"));
    }

    protected virtual void RemoveDeadTrees()
    {
        for (int i = trees.Count - 1; i >= 0; i--)
        {
            if (trees[i] == null) trees.RemoveAt(i);
        }
    }

    public override void DoingTask(WorkerCtrl workerCtrl)
    {
        switch (workerCtrl.workerTasks.TaskCurrent())
        {
            case TaskType.plantTree:
                this.PlantTree(workerCtrl);
                break;
            case TaskType.findTree2Chop:
                this.FindTree2Chop(workerCtrl);
                break;
            case TaskType.chopTree:
                this.ChopTree(workerCtrl);
                break;
            case TaskType.bringResourceBack:
                this.BringTreeBack(workerCtrl);
                break;
            case TaskType.goToWorkStation:
                this.GoToWorkStation(workerCtrl);
                break;
            default:
                if (this.IsTime2Work()) this.Planning(workerCtrl);
                break;
        }
    }

    protected virtual void Planning(WorkerCtrl workerCtrl)
    {
        if (!this.buildingCtrl.warehouse.IsFull())
        {
            workerCtrl.workerTasks.TaskAdd(TaskType.bringResourceBack);
            workerCtrl.workerTasks.TaskAdd(TaskType.chopTree);
            workerCtrl.workerTasks.TaskAdd(TaskType.findTree2Chop);
        }

        if (this.NeedMoreTree())
        {
            workerCtrl.workerMovement.SetTarget(null);
            workerCtrl.workerTasks.TaskAdd(TaskType.plantTree);
        }
    }

    protected virtual bool NeedMoreTree()
    {
        return this.trees.Count < this.treeMax;
    }

    protected virtual void PlantTree(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.workerMovement.isWorking) return;

        Transform target = workerCtrl.workerMovement.GetTarget();
        if (target == null)
        {
            target = this.GetPlantPlace();
            if (target == null) return;
            workerCtrl.workerMovement.SetTarget(target);
        }

        workerCtrl.workerTasks.taskWorking.GoOutBuilding();

        if (workerCtrl.workerMovement.IsClose2Target())
        {
            StartCoroutine(PlantingRoutine(workerCtrl, target));
        }
    }

    private IEnumerator PlantingRoutine(WorkerCtrl workerCtrl, Transform target)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.planting);
        yield return new WaitForSeconds(8f);

        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.planting);

        PoolManager.Instance.Despawn(target.gameObject);
        this.Planting(workerCtrl.transform);

        workerCtrl.workerMovement.SetTarget(null);

        if (!this.NeedMoreTree())
        {
            workerCtrl.workerTasks.TaskCurrentDone();
            workerCtrl.workerTasks.TaskAdd(TaskType.goToWorkStation);
        }
    }

    protected virtual void Planting(Transform trans)
    {
        GameObject treePrefab = this.GetTreePrefab();
        string path = PoolPrefabPath.Tree(treePrefab.name);
        GameObject treeObj = PoolManager.Instance.Spawn(path);

        SaveUtils.AssignID(treeObj, IDType.Tree);

        Vector3 offset = trans.forward * 0.5f;
        Vector3 placePos = trans.position + offset;

        Vector3 rayOrigin = placePos + Vector3.up * 5f;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 10f, groundLayer))
        {
            treeObj.transform.position = hit.point;
        }
        else
        {
            treeObj.transform.position = placePos;
        }

        treeObj.transform.rotation = trans.rotation;

        TreeCtrl treeCtrl = treeObj.GetComponent<TreeCtrl>();
        this.trees.Add(treeCtrl);
        TreeManager.Instance.TreeAdd(treeCtrl);
    }

    protected virtual GameObject GetTreePrefab()
    {
        return TreePrefabCache.GetRandomTree();
    }

    protected virtual Transform GetPlantPlace()
    {
        Vector3? newTreePos = this.RandomPlaceForTree();
        if (newTreePos == null) return null;

        GameObject treePlace = PoolManager.Instance.Spawn(PoolPrefabPath.Point("MaskPositionObject"));
        treePlace.transform.position = newTreePos.Value;
        return treePlace.transform;
    }

    protected virtual Vector3? RandomPlaceForTree()
    {
        const float minDistanceToObstacle = 2.5f;
        int obstacleLayerMask = LayerMask.GetMask("Building", "Nature", "Tree", "UnderConstruction");

        for (int i = 0; i < 20; i++)
        {
            Vector3 randomXZ = transform.position;
            randomXZ.x += Random.Range(-this.treeRange, this.treeRange);
            randomXZ.z += Random.Range(-this.treeRange, this.treeRange);

            Vector3 rayOrigin = randomXZ + Vector3.up * 10f;

            if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, 20f, groundLayer))
            {
                float dis = Vector3.Distance(transform.position, hit.point);
                if (dis < this.treeDistance) continue;

                Collider[] overlaps = Physics.OverlapSphere(hit.point, minDistanceToObstacle, obstacleLayerMask);
                if (overlaps.Length > 0) continue;

                if (!NavMesh.SamplePosition(hit.point, out NavMeshHit navHit, minDistanceToObstacle, NavMesh.AllAreas))
                    continue;

                if (Vector3.Distance(navHit.position, hit.point) > 0.5f) continue;

                return navHit.position;
            }
        }

        return null;
    }

    protected virtual void LoadNearByTrees()
    {
        List<TreeCtrl> allTrees = TreeManager.Instance.Trees();
        foreach (TreeCtrl tree in allTrees)
        {
            float dis = Vector3.Distance(tree.transform.position, transform.position);
            if (dis > this.treeRange) continue;
            this.TreeAdd(tree);
        }
    }

    public virtual void TreeAdd(TreeCtrl tree)
    {
        if (this.trees.Contains(tree)) return;
        this.trees.Add(tree);
    }

    protected virtual void ChopTree(WorkerCtrl workerCtrl)
    {
        if (workerCtrl.workerMovement.isWorking) return;
        StartCoroutine(Chopping(workerCtrl, workerCtrl.workerTasks.taskTarget));
    }

    private IEnumerator Chopping(WorkerCtrl workerCtrl, Transform tree)
    {
        workerCtrl.workerMovement.SetWorkingType(true, WorkingType.chopping);
        yield return new WaitForSeconds(this.workingSpeed);

        TreeCtrl treeCtrl = tree.GetComponent<TreeCtrl>();
        treeCtrl.treeLevel.ShowLastBuild();

        List<Resource> resources = treeCtrl.logwoodGenerator.TakeAll();
        treeCtrl.choper = null;

        this.trees.Remove(treeCtrl);
        TreeManager.Instance.TreeRemove(treeCtrl);

        workerCtrl.workerMovement.SetWorkingType(false, WorkingType.chopping);

        workerCtrl.workerTasks.taskTarget = null;
        workerCtrl.resCarrier.AddByList(resources);

        workerCtrl.workerTasks.TaskCurrentDone();

        yield return this.RemoveTree(tree);
    }

    protected virtual IEnumerator RemoveTree(Transform tree)
    {
        yield return new WaitForSeconds(this.treeRemoveSpeed);
        PoolManager.Instance.Despawn(tree.gameObject);
    }

    protected virtual void FindTree2Chop(WorkerCtrl workerCtrl)
    {
        WorkerTasks workerTasks = workerCtrl.workerTasks;
        if (workerTasks.inBuilding) workerTasks.taskWorking.GoOutBuilding();

        if (workerCtrl.workerTasks.taskTarget == null)
        {
            this.FindNearestTree(workerCtrl);
        }
        else
        {
            Transform target = workerCtrl.workerMovement.GetTarget();
            if (target == null) return;

            float distance = Vector3.Distance(workerCtrl.transform.position, target.position);
            if (distance <= 1.5f)
            {
                workerCtrl.workerTasks.TaskCurrentDone();
            }
        }
    }

    protected virtual void FindNearestTree(WorkerCtrl workerCtrl)
    {
        foreach (TreeCtrl treeCtrl in this.trees)
        {
            if (treeCtrl == null) continue;
            if (!treeCtrl.logwoodGenerator.IsAllResMax()) continue;
            if (treeCtrl.choper != null) continue;

            treeCtrl.choper = workerCtrl;
            workerCtrl.workerTasks.taskTarget = treeCtrl.transform;
            workerCtrl.workerMovement.SetTarget(treeCtrl.transform);
            return;
        }
    }

    protected virtual void BringTreeBack(WorkerCtrl workerCtrl)
    {
        WorkerTask taskWorking = workerCtrl.workerTasks.taskWorking;
        workerCtrl.workerMovement.SetMovingType(MovingType.carrying);
        taskWorking.GotoBuilding();

        if (!workerCtrl.workerMovement.IsClose2Target()) return;

        List<Resource> resources = workerCtrl.resCarrier.TakeAll();
        this.buildingCtrl.warehouse.AddByList(resources);
        workerCtrl.workerMovement.SetMovingType(MovingType.walking);
        taskWorking.GoIntoBuilding();

        workerCtrl.workerTasks.TaskCurrentDone();
    }
}
