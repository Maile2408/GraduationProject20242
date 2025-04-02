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
        foreach (var obj in activeItems) pool.Despawn(obj);

        activeItems.Clear();

        foreach (var info in allBuildings.Where(b => b.category == category))
        {
            GameObject go = pool.Spawn(contentParent);
            go.GetComponent<BuildingItem>().Setup(info);
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
}
