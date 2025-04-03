using UnityEngine;

public class BuildManager : SaiBehaviour
{
    public static BuildManager Instance;

    [Header("Build Settings")]
    [SerializeField] protected LayerMask groundMask;
    [SerializeField] protected float gridSize = 1f;

    [Header("State")]
    [SerializeField] protected Vector3 buildPos;
    [SerializeField] protected GameObject currentGhost;
    [SerializeField] protected BuildingInfo currentInfo;
    [SerializeField] protected bool isBuilding = false;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only 1 BuildManager allowed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isBuilding && currentGhost != null) UpdateGhostPosition();
    }

    public virtual void PrepareToBuild(BuildingInfo info)
    {
        CancelBuild();

        currentInfo = info;
        string path = PoolPrefabPath.Building(info.prefab.name);

        currentGhost = PoolManager.Instance.Spawn(path);
        if (currentGhost == null)
        {
            Debug.LogError($"[BuildManager] Cannot spawn ghost: {info.buildingName}");
            return;
        }

        Collider col = currentGhost.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (currentGhost.TryGetComponent(out LimitRadius limitRadius))
        {
            var bounds = currentGhost.GetComponentInChildren<Renderer>().bounds;
            limitRadius.ResizeToBounds(bounds.size);
            limitRadius.ClearColliders();
            limitRadius.UpdateHighlight();
        }

        isBuilding = true;
    }

    protected virtual void UpdateGhostPosition()
    {
        Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, groundMask))
        {
            Vector3 snapped = SnapToGrid(hit.point);
            buildPos = new Vector3(snapped.x, hit.point.y, snapped.z);

            currentGhost.transform.position = buildPos;

            currentGhost.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

            if (Input.GetMouseButtonDown(0)) PlaceBuilding();
            if (Input.GetMouseButtonDown(1)) CancelBuild();
        }
    }

    protected virtual Vector3 SnapToGrid(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float z = Mathf.Round(pos.z / gridSize) * gridSize;
        return new Vector3(x, pos.y, z);
    }

    protected virtual void PlaceBuilding()
    {
        if (currentGhost == null || currentInfo == null) return;

        if (currentGhost.TryGetComponent(out LimitRadius limitRadius) && limitRadius.IsCollided())
        {
            Debug.Log("Can't put the building because it is colliding!");
            return;
        }

        PoolManager.Instance.Despawn(currentGhost);

        GameObject underConstruction = PoolManager.Instance.Spawn(PoolPrefabPath.Building("UnderConstruction"));
        underConstruction.transform.position = buildPos;

        if (underConstruction.TryGetComponent(out AlignWithGround align))
        {
            align.Align();
        }

        if (underConstruction.TryGetComponent(out ConstructionCtrl ctrl))
        {
            ctrl.Setup(currentInfo);
        }

        ClearState();
    }

    protected virtual void CancelBuild()
    {
        if (currentGhost != null)
        {
            PoolManager.Instance.Despawn(currentGhost);
        }
        ClearState();
    }

    protected virtual void ClearState()
    {
        currentGhost = null;
        currentInfo = null;
        isBuilding = false;
    }
}
