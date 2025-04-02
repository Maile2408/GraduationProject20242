using UnityEngine;
using UnityEngine.UI;

public class BuildingItemUI : MonoBehaviour, IPoolable
{
    public Image icon;
    private BuildingInfo info;
    public GameObject lockIcon;

    public void Setup(BuildingInfo data)
    {
        info = data;

        if (info == null || !info.isUnlocked)
        {
            icon.sprite = null;
            icon.enabled = false;
            lockIcon.SetActive(true);
        }
        else
        {
            icon.sprite = info.icon;
            icon.enabled = true;
            lockIcon.SetActive(false);
        }
    }

    public void OnSpawn() => gameObject.SetActive(true);
    public void OnDespawn() => gameObject.SetActive(false);
}
