using UnityEngine;

public class DestroyManager : SaiBehaviour
{
    public static DestroyManager Instance;

    [Header("Settings")]
    [SerializeField] private Texture2D shovelCursor;
    [SerializeField] private Vector2 hotspot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private LayerMask destroyableMask; // Building | UnderConstruction

    [Header("State")]
    [SerializeField] private bool isDestroying = false;
    private BuildDestroyable currentTarget;
    private BuildDestroyable targetToDestroy;
    private Camera cam;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void Start()
    {
        base.Start();
        cam = GodModeCtrl.Instance != null ? GodModeCtrl.Instance._camera : Camera.main;
    }

    protected override void Update()
    {
        base.Update();
        if (!isDestroying) return;

        UpdateRaycastTarget();

        if (Input.GetMouseButtonDown(1))
        {
            CancelDestroyMode();
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (currentTarget != null)
            {
                if (!currentTarget.CanBeDestroyed())
                {
                    GameMessage.Warning("This building cannot be destroyed");
                    return;
                }

                targetToDestroy = currentTarget;

                CancelDestroyMode();

                ConfirmationPopupController.OnYesCallback = OnConfirmYes;
                ConfirmationPopupController.OnNoCallback = OnConfirmNo;
                ConfirmationPopupController.Message = "Are you sure you want to destroy this building?";

                ScreenManager.Add<ConfirmationPopupController>(ConfirmationPopupController.NAME);
            }
        }
    }

    private void UpdateRaycastTarget()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red, 0.2f);

        currentTarget = null;

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, destroyableMask))
        {
            var destroyable = hit.collider.GetComponentInParent<BuildDestroyable>();
            if (destroyable != null)
            {
                currentTarget = destroyable;
            }
        }
    }

    public void EnterDestroyMode()
    {
        isDestroying = true;
        Cursor.SetCursor(shovelCursor, hotspot, cursorMode);

        GameMessage.Info("Entered destroy mode");
        GameMessage.Guide("Left-click a building to destroy it\nRight-click to exit destroy mode");
        //Debug.Log("[DestroyManager] Entered destroy mode.");
    }

    public void CancelDestroyMode()
    {
        isDestroying = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        currentTarget = null;

        GameMessage.Info("Exited destroy mode");
        //Debug.Log("[DestroyManager] Exited destroy mode.");
    }

    private void OnConfirmYes()
    {
        if (targetToDestroy != null)
        {
            string name = "Unknown";

            var ctrl = targetToDestroy.GetComponent<BuildingCtrl>();
            if (ctrl != null && ctrl.buildingInfo != null) name = ctrl.buildingInfo.buildingName;
            else name = targetToDestroy.name;

            targetToDestroy.Destroy();
            AudioManager.Instance.PlayBuildingDestroy();

            GameMessage.Success($"Destroyed: {name}");
            //Debug.Log("[DestroyManager] Destroyed: " + name);
        }

        ClearCallbacks();
        CancelDestroyMode();
    }

    private void OnConfirmNo()
    {
        //Debug.Log("[DestroyManager] Destruction cancelled.");
        ClearCallbacks();
        CancelDestroyMode();
    }

    private void ClearCallbacks()
    {
        ConfirmationPopupController.OnYesCallback = null;
        ConfirmationPopupController.OnNoCallback = null;
    }

    public bool IsInDestroyMode() => isDestroying;
}
