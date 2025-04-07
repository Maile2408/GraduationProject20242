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
    [SerializeField] private LayerMask destroyableMask;

    [Header("State")]
    [SerializeField] private bool isDestroying = false;
    private BuildDestroyable currentTarget;
    private Camera cam;

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

    protected override void Start()
    {
        base.Start();
        cam = GodModeCtrl.Instance != null ? GodModeCtrl.Instance._camera : Camera.main;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!isDestroying) return;

        UpdateRaycastTarget();
    }

    protected override void Update()
    {
        base.Update();

        if (!isDestroying) return;

        // Cancel by right-click
        if (Input.GetMouseButtonDown(1))
        {
            CancelDestroyMode();
            return;
        }

        // Cancel if click UI
        if (Input.GetMouseButtonDown(0) && IsPointerOverUI())
        {
            CancelDestroyMode();
            return;
        }

        // Left click to confirm destroy
        if (Input.GetMouseButtonDown(0) && currentTarget != null)
        {
            ConfirmationPopupController.OnYesCallback = OnConfirmYes;
            ConfirmationPopupController.OnNoCallback = OnConfirmNo;
            ConfirmationPopupController.Message = "Are you sure you want to destroy this building?";

            ScreenManager.Add<ConfirmationPopupController>(ConfirmationPopupController.NAME);
        }
    }

    private void UpdateRaycastTarget()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 999f, Color.red, 0.2f);

        if (Physics.Raycast(ray, out RaycastHit hit, 999f, destroyableMask))
        {
            Debug.Log($"[DestroyManager] Hit object: {hit.collider.name}, Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
            var destroyable = hit.collider.GetComponentInParent<BuildDestroyable>();
            if (destroyable != null && destroyable.CanBeDestroyed())
            {
                currentTarget = destroyable;
                return;
            }
        }

        currentTarget = null;
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
