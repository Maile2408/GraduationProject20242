using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuManager : MonoBehaviour
{
    [Header("Category")]
    [SerializeField] List<CategoryButtonGroup> categoryGroups;

    [Header("Building List")]
    [SerializeField] Transform contentParent;
    [SerializeField] GameObject buildingItemPrefab;
    [SerializeField] List<BuildingInfo> allBuildings;

    private List<GameObject> activeItems = new();

    private string buildingItemPath;

    private void Awake()
    {
        allBuildings = Resources.LoadAll<BuildingInfo>(PoolPrefabPath.BuildingInfo(""))
        .OrderBy(b => b.name)
        .ToList();
        buildingItemPath = PoolPrefabPath.BuildingItem(buildingItemPrefab.name);
    }

    private void Start()
    {
        ShowCategory(BuildingCategory.Needs);
    }

    public void ShowCategory(BuildingCategory category)
    {
        foreach (var obj in activeItems)
        {
            PoolManager.Instance.Despawn(obj);
        }
        activeItems.Clear();

        foreach (var info in allBuildings.Where(b => b.category == category))
        {
            GameObject go = PoolManager.Instance.Spawn(buildingItemPath, contentParent);
            go.transform.localScale = Vector3.one;
            go.GetComponent<BuildingItem>().Setup(info, OnBuildingItemClick);
            activeItems.Add(go);
        }

        foreach (var group in categoryGroups)
        {
            var img = group.GetComponent<Image>();
            Color c = img.color;
            c.a = (group.category == category) ? 1f : 0.2f;
            img.color = c;
        }
    }

    private void OnBuildingItemClick(BuildingInfo info)
    {
        BuildingInfoPopupController.pendingInfo = info;
        ScreenManager.Close();
        ScreenManager.Add<BuildingInfoPopupController>(BuildingInfoPopupController.NAME);
    }
}
