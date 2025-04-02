using UnityEngine;
using UnityEngine.UI;

public class BuildingItemUI : MonoBehaviour, IPoolable
{
    public Image icon;

    private BuildingInfo info;

    public void Setup(BuildingInfo data)
    {
        info = data;
        icon.sprite = info.icon;
    }

    public void OnSpawn()
    {
        // Nếu cần hiệu ứng hiện ra
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
