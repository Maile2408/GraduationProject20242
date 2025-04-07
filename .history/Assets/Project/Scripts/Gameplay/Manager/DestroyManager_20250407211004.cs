using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DestroyManager : SaiBehaviour
{
    public static DestroyManager Instance;

    [Header("Destroy Settings")]
    [SerializeField] private LayerMask groundMask;            // Layer for ground detection
    [SerializeField] private float detectRadius = 0.5f;       // Radius for detecting nearby destroyables
    [SerializeField] private Texture2D shovelCursor;          // Shovel cursor icon
    [SerializeField] private Vector2 hotspot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

    [Header("State")]
    [SerializeField] private bool isDestroying = false;
    [SerializeField] private Vector3 destroyPos;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only 1 DestroyManager allowed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (isDestroying)
        {
            UpdateDestroyLogic();
        }
    }

    public void EnterDestroyMode()
    {
        isDestroying = true;
        Cursor.SetCursor(shovelCursor, hotspot, cursorMode);
        Debug.Log("[DestroyManager] Entered destroy mode");
    }

    public void CancelDestroyMode()
    {
        isDestroying = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        Debug.Log("[DestroyManager] Exited destroy mode");
    }

    protected virtual void UpdateDestroyLogic()
    {
        // Cancel when right-click
        if (Input.GetMouseButtonDown(1))
        {
            CancelDestroyMode();
            return;
        }

        // Cancel if clicking on UI
        if (IsPointerOverUI() && Input.GetMouseButtonDown(0))
        {
            CancelDestroyMode();
            return;
        }

        // Raycast to ground
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 999f, groundMask)) return;

        destroyPos = hit.point;

        // Left click: try to destroy nearby object
        if (Input.GetMouseButtonDown(0))
        {
            TryDestroyAt(destroyPos);
        }
    }

    protected virtual void TryDestroyAt(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, detectRadius);
        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out BuildDestroyable destroyable) && destroyable.CanBeDestroyed())
            {
                destroyable.Destroy();
                Debug.Log("[DestroyManager] Destroyed: " + destroyable.gameObject.name);
                CancelDestroyMode();
                return;
            }
        }

        Debug.Log("[DestroyManager] No destroyable found near: " + position);
    }

    // Precise UI detection using event system
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
