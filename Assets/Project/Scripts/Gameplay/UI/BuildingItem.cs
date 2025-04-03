using System;
using UnityEngine;
using UnityEngine.UI;

public class BuildingItem : MonoBehaviour, IPoolable
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject lockIcon;

    private BuildingInfo info;
    private Action<BuildingInfo> onClick;

    public void Setup(BuildingInfo data)
    {
        info = data;

        bool hasData = info.icon != null;
        bool isLocked = !info.isUnlocked;

        if (icon != null)
        {
            if (!hasData)
            {
                icon.sprite = null;
                icon.color = new Color(1f, 1f, 1f, 0.2f);
            }
            else
            {
                icon.sprite = info.icon;
                icon.color = Color.white;
            }
        }

        if (lockIcon != null)
        {
            lockIcon.SetActive(isLocked);
        }

        var button = GetComponent<Button>();
        if (button != null)
        {
            button.interactable = hasData && !isLocked;
        }

        var bg = GetComponent<Image>();
        if (bg != null)
        {
            bg.color = (!hasData) ? new Color(1f, 1f, 1f, 0.2f) : Color.white;
        }
    }

    public void Setup(BuildingInfo data, Action<BuildingInfo> onClickCallback)
    {
        onClick = onClickCallback;
        Setup(data);

        var button = GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() =>
            {
                if (data.isUnlocked)
                    onClick?.Invoke(data);
            });
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
