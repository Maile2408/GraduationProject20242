using System;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonGroup : MonoBehaviour
{
    public BuildingCategory category;
    public BuildMenuManager menu;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => menu.ShowCategory(category));
    }
}
