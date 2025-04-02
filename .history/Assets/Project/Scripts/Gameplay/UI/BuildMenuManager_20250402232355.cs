using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildMenuManager : MonoBehaviour
{
    [Header("Category")]
    public List<CategoryButtonGroup> categoryGroups;

    [Header("Building List")]
    public Transform contentParent;
    public GameObject buildingItemPrefab;
    public List<BuildingInfo> allBuildings;

    private ObjectPool pool;
    private List<GameObject> activeItems = new();

    private void Awake()
    {
        pool = new ObjectPool(buildingItemPrefab, contentParent);
    }

    private void Start()
    {
        ShowCategory(BuildingCategory.Needs);
    }

    public void ShowCategory(BuildingCategory category)
    {
        // 1. Trả các item cũ về pool
        foreach (var obj in activeItems)
            pool.Despawn(obj);

        activeItems.Clear();

        // 2. Hiển thị item mới
        foreach (var info in allBuildings.Where(b => b.category == category))
        {
            GameObject go = pool.Spawn(contentParent);
            go.GetComponent<BuildingItemUI>().Setup(info);
            activeItems.Add(go);
        }

        // 3. Cập nhật màu alpha button
        foreach (var group in categoryGroups)
        {
            var img = group.GetComponent<Image>();
            Color c = img.color;
            c.a = (group.category == category) ? 1f : 0.2f;
            img.color = c;
        }
    }
}
