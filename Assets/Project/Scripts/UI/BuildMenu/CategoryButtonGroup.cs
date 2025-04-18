using UnityEngine;
using UnityEngine.UI;

public class CategoryButtonGroup : MonoBehaviour
{
    public BuildingCategory category;
    public BuildMenuManager menu;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            AudioManager.Instance.PlayButtonTap();
            menu.ShowCategory(category);
        });
    }
}
