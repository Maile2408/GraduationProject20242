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
    [SerializeField] private LayerMask destroyableMask; // Only Building, UnderConstruction

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

        // Click on UI → cancel
        if (IsPointerOverUI() && Input.GetMouseButtonDown(0))
        {
            CancelDestroyMode();
            return;
        }

        // Click to try destroy
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = GodModeCtrl.Instance._camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, destroyableMask)) return;

            if (hit.collider.TryGetComponent(out BuildDestroyable destroyable) && destroyable.CanBeDestroyed())
            {
                currentTarget = destroyable;

                // Assign static callbacks
                ConfirmationPopupController.OnYesCallback = OnConfirmYes;
                ConfirmationPopupController.OnNoCallback = OnConfirmNo;

                // Show popup via SS
                ScreenManager.Add<ConfirmationPopupController>(ConfirmationPopupController.NAME);
            }
            else
            {
                Debug.Log("[DestroyManager] No valid destroyable target.");
                CancelDestroyMode();
            }
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
        Debug.Log("[DestroyManager] Cancelled by user.");
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

        var results = new List<RaycastResult>();
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
