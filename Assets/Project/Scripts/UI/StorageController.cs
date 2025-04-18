using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StorageController : MonoBehaviour, IKeyBack
{
    public const string NAME = "Storage";

    [Header("UI References")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private TextMeshProUGUI txtTotalStorage;

    private Dictionary<ResourceName, TextMeshProUGUI> resourceTexts = new();

    private void OnEnable()
    {
        CacheResourceTexts();
        UpdateStorageDisplay();

        WarehouseWH.OnStorageChanged += UpdateStorageDisplay;
    }

    private void OnDisable()
    {
        WarehouseWH.OnStorageChanged -= UpdateStorageDisplay;
    }

    private void CacheResourceTexts()
    {
        resourceTexts.Clear();

        foreach (Transform child in contentParent)
        {
            if (System.Enum.TryParse(child.name, out ResourceName resName))
            {
                TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();
                if (text != null)
                {
                    resourceTexts[resName] = text;
                }
            }
        }
    }

    private void UpdateStorageDisplay()
    {
        var totals = StorageResourceManager.Instance.GetTotalResources();

        foreach (var pair in resourceTexts)
        {
            float value = totals.GetValueOrDefault(pair.Key, 0);
            pair.Value.text = $"{value:0}";
        }

        var (used, capacity) = StorageResourceManager.Instance.GetTotalUsedAndCapacity();
        txtTotalStorage.text = $"{used:0}/{capacity:0}";
    }

    public void OnCloseButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
