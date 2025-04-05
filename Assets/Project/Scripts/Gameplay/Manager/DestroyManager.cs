using UnityEngine;
using UnityEngine.EventSystems;

public class DestroyManager : SaiBehaviour
{
    public static DestroyManager Instance;

    [Header("Settings")]
    [SerializeField] private Texture2D shovelCursor;
    [SerializeField] private Vector2 hotspot = Vector2.zero;
    [SerializeField] private CursorMode cursorMode = CursorMode.Auto;

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

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            TryDestroy();
        }

        if (Input.GetMouseButtonDown(1))
        {
            CancelDestroyMode();
        }
    }

    public void EnterDestroyMode()
    {
        isDestroyMode = true;
        Cursor.SetCursor(shovelCursor, hotspot, cursorMode);
    }

    public void CancelDestroyMode()
    {
        isDestroyMode = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    protected virtual void TryDestroy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit, 1000f)) return;

        if (hit.collider.TryGetComponent(out BuildDestroyable destroyable) && destroyable.CanBeDestroyed())
        {
            destroyable.Destroy();
            CancelDestroyMode(); 
        }
    }

    public bool IsInDestroyMode() => isDestroyMode;
}
