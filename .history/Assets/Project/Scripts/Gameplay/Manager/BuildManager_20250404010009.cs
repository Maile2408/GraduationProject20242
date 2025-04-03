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

    protected override void Start()
    {
        base.Start();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isBuilding && currentGhost != null) this.UpdateGhostPosition();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    public virtual void PrepareToBuild(BuildingInfo info)
    {
        this.CancelBuild();

        this.currentInfo = info;
        string path = PoolPrefabPath.Building(info.prefab.name);

        this.currentGhost = PoolManager.Instance.Spawn(path);
        if (this.currentGhost == null)
        {
            Debug.LogError($"[BuildManager] Cannot spawn ghost: {info.buildingName}");
            return;
        }

        Collider col = this.currentGhost.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (this.currentGhost.TryGetComponent(out LimitRadius limitRadius))
        {
            var bounds = this.currentGhost.GetComponentInChildren<Renderer>().bounds;
            Vector3 size = bounds.size;

            limitRadius.ResizeToBounds(size);

            limitRadius.ClearColliders();
            limitRadius.UpdateHighlight();
        }

        this.isBuilding = true;
    }

    protected virtual void UpdateGhostPosition()
    {
        Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f, groundMask))
        {
            this.buildPos = this.SnapToGrid(hit.point);
            this.currentGhost.transform.position = this.buildPos;

            if (Input.GetMouseButtonDown(0)) this.PlaceBuilding();
            if (Input.GetMouseButtonDown(1)) this.CancelBuild();
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
        if (this.currentGhost == null || this.currentInfo == null) return;

        if (this.currentGhost.TryGetComponent(out LimitRadius limitRadius))
        {
            if (limitRadius.IsCollided())
            {
                Debug.Log("Can't put it because it is colliding!");
                return;
            }
        }

        PoolManager.Instance.Despawn(this.currentGhost);

        GameObject underConstruction = PoolManager.Instance.Spawn(PoolPrefabPath.Building("UnderConstruction"));
        underConstruction.transform.position = this.buildPos;

        if (underConstruction.TryGetComponent(out ConstructionCtrl ctrl))
        {
            ctrl.Setup(this.currentInfo);
        }

        this.ClearState();
    }

    protected virtual void CancelBuild()
    {
        if (this.currentGhost != null)
        {
            PoolManager.Instance.Despawn(this.currentGhost);
        }

        this.ClearState();
    }

    protected virtual void ClearState()
    {
        this.currentGhost = null;
        this.currentInfo = null;
        this.isBuilding = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (!this.isBuilding) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Camera.main.transform.position, this.buildPos);
    }
}
