using UnityEngine;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour, IPoolable
{
    [SerializeField] Image icon;
    private BuildingInfo info;
    [SerializeField] GameObject lockIcon;

    public void Setup(BuildingInfo data)
    {
        info = data;

        bool locked = (info == null || !info.isUnlocked);

        icon.sprite = locked ? null : info.icon;
        icon.color = locked ? new Color(1, 1, 1, 0.2f) : Color.white;

        lockIcon.SetActive(locked);

        var button = GetComponent<Button>();
        button.interactable = !locked;

        var bg = GetComponent<Image>();
        if (bg != null)
            bg.color = locked ? new Color(1, 1, 1, 0.2f) : Color.white;
    }

    public void OnSpawn() => gameObject.SetActive(true);
    public void OnDespawn() => gameObject.SetActive(false);
}
