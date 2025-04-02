using System;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonGroup : MonoBehaviour
{
    [SerializeField] BuildingCategory category;
    [SerializeField] BuildMenuManager menu;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => menu.ShowCategory(category));
    }
}
