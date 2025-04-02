using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuManager : MonoBehaviour
{
    [Header("Category Buttons")]
    public List<CategoryButtonGroup> categoryGroups;

    [Header("Building List")]
    public Transform contentParent;
    public GameObject buildingItemPrefab;
    public List<BuildingInfo> allBuildings;

    private void Start()
    {
        ShowCategory(BuildingCategory.Needs);
    }

    public void ShowCategory(BuildingCategory category)
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        foreach (var info in allBuildings.Where(b => b.category == category))
        {
            var go = Instantiate(buildingItemPrefab, contentParent);
            go.GetComponentInChildren<Image>().sprite = info.icon;
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
