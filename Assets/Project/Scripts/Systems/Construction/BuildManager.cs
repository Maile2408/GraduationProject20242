using UnityEngine;

public class BuildManager : SaiBehaviour
{
    public static BuildManager Instance;

    [Header("Build Settings")]
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gridSize = 1f;

    [Header("State")]
    [SerializeField] private Vector3 buildPos;
    [SerializeField] private GameObject currentGhost;
    [SerializeField] private BuildingInfo currentInfo;
    [SerializeField] private bool isBuilding = false;
    [SerializeField] private Quaternion buildRotation = Quaternion.identity;

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

        buildRotation = Quaternion.identity;
        currentGhost.transform.rotation = buildRotation;

        // Resize collider + visual highlight
        if (currentGhost.TryGetComponent(out LimitRadius limitRadius))
        {
            Bounds bounds = new Bounds(currentGhost.transform.position, Vector3.zero);
            foreach (Renderer r in currentGhost.GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(r.bounds);
            }

            limitRadius.ResizeToBounds(bounds.size);
            limitRadius.ClearColliders(); // important for pooled ghost
        }

        isBuilding = true;

        GameMessage.Guide("Press [R] to rotate the building");
    }

    protected virtual void UpdateGhostPosition()
    {
        Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.green, 0.2f);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, groundMask))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (hitObject.layer != LayerMask.NameToLayer("Ground")) return;

            Vector3 snapped = SnapToGrid(hit.point);
            buildPos = new Vector3(snapped.x, hit.point.y, snapped.z);

            currentGhost.transform.position = buildPos;
            currentGhost.transform.rotation = buildRotation;

            if (Input.GetMouseButtonDown(0)) PlaceBuilding();
            if (Input.GetMouseButtonDown(1)) CancelBuild();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            AudioManager.Instance.PlayBuildRotate();
            RotateGhost();
        }
    }

    protected virtual void RotateGhost()
    {
        buildRotation *= Quaternion.Euler(0, 90f, 0);
        if (currentGhost != null)
        {
            currentGhost.transform.rotation = buildRotation;
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

        // Check for collision
        if (currentGhost.TryGetComponent(out LimitRadius limitRadius) && limitRadius.IsCollided())
        {
            Debug.Log("Can't place the building due to collision.");
            return;
        }

        Vector3 highlightScale = Vector3.one;
        if (currentGhost.TryGetComponent(out LimitRadius limit))
        {
            highlightScale = limit.highlightRenderer.transform.localScale;
        }

        AudioManager.Instance.PlayBuildingPlace();

        PoolManager.Instance.Despawn(currentGhost);
        CurrencyManager.Instance.SpendCoin(currentInfo.coin);  
        DeductResourcesOnPlacement(currentInfo);
        CityLevelManager.Instance.AddXP(125);
        GameMessage.Success($"Construction Complete: {currentInfo.buildingName}! +125 XP");               

        GameObject underConstruction = PoolManager.Instance.Spawn(PoolPrefabPath.Building("UnderConstruction"));
        underConstruction.transform.position = buildPos;
        underConstruction.transform.rotation = buildRotation;
        underConstruction.transform.localScale = highlightScale;

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

    private void DeductResourcesOnPlacement(BuildingInfo info)
    {
        foreach (var res in info.cost)
        {
            float targetToDeduct = Mathf.Max(0, res.number - 1f);
            float remaining = targetToDeduct;

            foreach (var building in BuildingManager.Instance.BuildingCtrls())
            {
                if (building.warehouse is not WarehouseWH wh) continue;

                var holder = wh.GetResource(res.name);
                if (holder == null || holder.Current() <= 0) continue;

                float available = holder.Current();
                float take = Mathf.Min(available, remaining);
                holder.Deduct(take);

                remaining -= take;
                if (remaining <= 0) break;
            }

            Debug.Log($"[BuildManager] Deducted {targetToDeduct}/{res.number} of {res.name}");
        }
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
        buildRotation = Quaternion.identity;
    }
}