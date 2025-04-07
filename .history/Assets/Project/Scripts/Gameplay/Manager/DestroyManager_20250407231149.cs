using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DestroyManager : SaiBehaviour
{
    public static DestroyManager Instance;

    [Header("Settings")]
    [SerializeField] private Texture2D shovelCursor;
    [SerializeField] private Vector2 hotspot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private LayerMask destroyableMask; // e.g., Building, UnderConstruction

    [Header("State")]
    [SerializeField] private bool isDestroying = false;
    private BuildDestroyable currentTarget;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only one DestroyManager allowed.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void Update()
    {
        base.Update();
        if (!isDestroying) return;

        // Right-click to cancel
        if (Input.GetMouseButtonDown(1))
        {
            CancelDestroyMode();
            return;
        }

        // Left-click: ignore if clicking on UI
        if (Input.GetMouseButtonDown(0) && IsPointerOverUI())
        {
            CancelDestroyMode();
            return;
        }

        // Left-click: try to destroy
        if (Input.GetMouseButtonDown(0))
        {
            TryRaycastDestroy();
        }
    }

    private void TryRaycastDestroy()
    {
        Camera cam = GodModeCtrl.Instance != null ? GodModeCtrl.Instance._camera : Camera.main;
        if (cam == null)
        {
            Debug.LogError("[DestroyManager] No valid camera found.");
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red, 1f);

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, destroyableMask))
        {
            Debug.Log("[DestroyManager] Raycast missed.");
            return;
        }

        Debug.Log($"[DestroyManager] Hit object: {hit.collider.name}");

        var destroyable = hit.collider.GetComponentInParent<BuildDestroyable>();
        if (destroyable != null && destroyable.CanBeDestroyed())
        {
            currentTarget = destroyable;

            ConfirmationPopupController.OnYesCallback = OnConfirmYes;
            ConfirmationPopupController.OnNoCallback = OnConfirmNo;
            ConfirmationPopupController.Message = "Are you sure you want to destroy this building?";

            ScreenManager.Add<ConfirmationPopupController>(ConfirmationPopupController.NAME);
        }
        else
        {
            Debug.LogWarning("[DestroyManager] Object is not destroyable or not allowed to be destroyed.");
        }
    }

    public void EnterDestroyMode()
    {
        isDestroying = true;
        Cursor.SetCursor(shovelCursor, hotspot, cursorMode);
        Debug.Log("[DestroyManager] Entered destroy mode.");
    }

    public void CancelDestroyMode()
    {
        isDestroying = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        currentTarget = null;
        Debug.Log("[DestroyManager] Exited destroy mode.");
    }

    private void OnConfirmYes()
    {
        if (currentTarget != null)
        {
            currentTarget.Destroy();
            Debug.Log("[DestroyManager] Destroyed: " + currentTarget.name);
        }

        ClearCallbacks();
        CancelDestroyMode();
    }

    private void OnConfirmNo()
    {
        Debug.Log("[DestroyManager] Destruction cancelled.");
        ClearCallbacks();
        CancelDestroyMode();
    }

    private void ClearCallbacks()
    {
        ConfirmationPopupController.OnYesCallback = null;
        ConfirmationPopupController.OnNoCallback = null;
    }

    private bool IsPointerOverUI()
    {
        if (EventSystem.current == null) return false;

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }

        return false;
    }

    public bool IsInDestroyMode() => isDestroying;
}
