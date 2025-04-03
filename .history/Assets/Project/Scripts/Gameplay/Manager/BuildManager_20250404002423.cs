using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance { get; private set; }

    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gridSize = 1f;

    private BuildingInfo currentInfo;
    private GameObject currentGhost;
    private bool isPlacing = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (!isPlacing || currentGhost == null) return;

        UpdateGhostPosition();

        if (Input.GetMouseButtonDown(0)) PlaceBuilding();
        if (Input.GetMouseButtonDown(1)) CancelBuild();
    }

    public void PrepareToBuild(BuildingInfo info)
    {
        CancelBuild();

        currentInfo = info;
        string path = PoolPrefabPath.Building(info.prefab.name);

        currentGhost = PoolManager.Instance.Spawn(path);
        if (currentGhost == null)
        {
            Debug.LogError($"[BuildManager] Failed to spawn ghost for {info.buildingName}");
            return;
        }

        currentGhost.GetComponent<Collider>().enabled = false;
        currentGhost.SetActive(true);
        isPlacing = true;
    }

    private void UpdateGhostPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, 1000f, groundMask))
        {
            currentGhost.transform.position = SnapToGrid(hit.point);
        }
    }

    private Vector3 SnapToGrid(Vector3 pos)
    {
        float x = Mathf.Round(pos.x / gridSize) * gridSize;
        float z = Mathf.Round(pos.z / gridSize) * gridSize;
        return new Vector3(x, pos.y, z);
    }

    private void PlaceBuilding()
    {
        if (currentInfo == null || currentGhost == null) return;

        Vector3 placePosition = currentGhost.transform.position;

        PoolManager.Instance.Despawn(currentGhost);

        GameObject constructionZone = PoolManager.Instance.Spawn(PoolPrefabPath.Building("UnderConstruction"));
        constructionZone.transform.position = placePosition;

        if (constructionZone.TryGetComponent(out ConstructionCtrl constructionCtrl))
        {
            constructionCtrl.Setup(currentInfo);
        }

        ResetState();
    }

    private void CancelBuild()
    {
        if (currentGhost != null)
        {
            PoolManager.Instance.Despawn(currentGhost);
        }
        ResetState();
    }

    private void ResetState()
    {
        currentInfo = null;
        currentGhost = null;
        isPlacing = false;
    }
}
