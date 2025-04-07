using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyManager : SaiBehaviour
{
    public static DestroyManager Instance;

    [Header("Settings")]
    [SerializeField] private Texture2D shovelCursor;           
    [SerializeField] private Vector2 hotspot = Vector2.zero;    
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
    [SerializeField] private LayerMask groundMask;              
    [SerializeField] private float detectRadius = 0.5f;         

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

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
            {
                CancelDestroyMode();
            }
            return;
        }

        // Left-click to try destroy
        if (Input.GetMouseButtonDown(0))
        {
            TryDestroy();
        }

        // Right-click to cancel
        if (Input.GetMouseButtonDown(1))
        {
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

        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f, groundMask))
        {
            Debug.Log("[DestroyManager] Raycast did not hit ground");
            return;
        }

        Vector3 point = hit.point;

        // Check nearby objects around the hit point
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

        Debug.Log("[DestroyManager] No destroyable object found near click");
    }

    public bool IsInDestroyMode() => isDestroyMode;
}
