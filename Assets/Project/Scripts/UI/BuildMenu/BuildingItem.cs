using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour, IPoolable
{
    [Header("UI")]
    [SerializeField] private Image icon;
    [SerializeField] private GameObject lockIcon;

    private BuildingInfo info;
    private Action<BuildingInfo> onClick;

    public void Setup(BuildingInfo data, Action<BuildingInfo> onClickCallback = null)
    {
        info = data;
        onClick = onClickCallback;

        bool hasIcon = info.icon != null;
        bool isUnlocked = UnlockManager.Instance.IsUnlocked(info.buildingID);

        // Icon setup
        if (icon != null)
        {
            icon.sprite = hasIcon ? info.icon : null;
            icon.color = hasIcon ? Color.white : new Color(1f, 1f, 1f, 0.2f);
        }

        // Lock icon
        if (lockIcon != null)
        {
            lockIcon.SetActive(!isUnlocked);
        }

        // Button interactability
        var button = GetComponent<Button>();
        if (button != null)
        {
            button.interactable = hasIcon && isUnlocked;
            button.onClick.RemoveAllListeners();

            if (onClick != null && isUnlocked)
            {
                button.onClick.AddListener(() => onClick.Invoke(info));
            }
        }

        // Background UI (optional)
        var bg = GetComponent<Image>();
        if (bg != null)
        {
            bg.color = hasIcon ? Color.white : new Color(1f, 1f, 1f, 0.2f);
        }
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        gameObject.SetActive(false);
    }
}
