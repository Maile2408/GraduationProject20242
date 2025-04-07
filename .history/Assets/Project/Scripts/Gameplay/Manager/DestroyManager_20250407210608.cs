using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class DestroyManager : SaiBehaviour
{
    public static DestroyManager Instance;

    [Header("Settings")]
    [SerializeField] private Texture2D shovelCursor;              // Cursor for destroy mode
    [SerializeField] private Vector2 hotspot = Vector2.zero;      // Cursor hotspot
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private LayerMask groundMask;                // Layer for ground raycast
    [SerializeField] private float detectRadius = 0.5f;           // Radius to find destroyable objects

    [Header("State")]
    [SerializeField] private bool isDestroyMode = false;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Only one DestroyManager allowed");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    protected override void Update()
    {
        base.Update();

        if (!isDestroyMode) return;

        // Cancel destroy mode when clicking on UI (left click only)
        if (IsPointerOverUI())
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("[DestroyManager] Clicked UI → Cancel destroy mode");
                CancelDestroyMode();
            }
            // Do NOT return here → still allow right-click to cancel
        }

        // Left click: try to destroy something
        if (Input.GetMouseButtonDown(0))
        {
            TryDestroy();
        }

        // Right click: always cancel destroy mode
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("[DestroyManager] Right-click → Cancel destroy mode");
            CancelDestroyMode();
        }
    }

    public void EnterDestroyMode()
    {
        isDestroyMode = true;
        Cursor.SetCursor(shovelCursor, hotspot, cursorMode);
        Debug.Log("[DestroyManager] Entered destroy mode");
    }

    public void CancelDestroyMode()
    {
        isDestroyMode = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
        Debug.Log("[DestroyManager] Exited destroy mode");
    }

    protected virtual void TryDestroy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Raycast down to ground
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            Debug.Log("[DestroyManager] Raycast did not hit ground");
            return;
        }

        Vector3 point = hit.point;

        // Look for nearby destroyable objects
        Collider[] colliders = Physics.OverlapSphere(point, detectRadius);

        foreach (var col in colliders)
        {
            if (col.TryGetComponent(out BuildDestroyable destroyable))
            {
                if (destroyable.CanBeDestroyed())
                {
                    destroyable.Destroy();
                    Debug.Log("[DestroyManager] Destroyed: " + destroyable.gameObject.name);
                    CancelDestroyMode();
                    return;
                }
            }
        }

        Debug.Log("[DestroyManager] No destroyable object found near clicked point");
    }

    // Accurate check: only return true if pointer is over actual UI layer
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

    public bool IsInDestroyMode() => isDestroyMode;
}
