using System;
using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonGroup : MonoBehaviour
{
    public BuildingCategory category;
    public BuildMenuManager menu;

    internal object GetComponent<T>()
    {
        throw new NotImplementedException();
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => menu.ShowCategory(category));
    }
}
