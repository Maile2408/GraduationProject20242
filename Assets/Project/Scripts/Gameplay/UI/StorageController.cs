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

        Warehouse.OnStorageChanged += UpdateStorageDisplay;
    }

    private void OnDisable()
    {
        Warehouse.OnStorageChanged -= UpdateStorageDisplay;
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
        float totalCapacity = 0f;
        Dictionary<ResourceName, float> totals = new();

        foreach (var building in BuildingManager.Instance.BuildingCtrls())
        {
            if (building.warehouse is not WarehouseWH wh) continue;

            totalCapacity += wh.GetStorageCapacity();

            foreach (var holder in wh.GetStockedResources())
            {
                ResourceName res = holder.Name();
                float amount = holder.Current();

                if (!totals.ContainsKey(res)) totals[res] = 0;
                totals[res] += amount;
            }
        }

        foreach (var pair in resourceTexts)
        {
            float value = totals.GetValueOrDefault(pair.Key, 0);
            pair.Value.text = $"{value:0}";
        }

        float totalUsed = 0;
        foreach (var val in totals.Values) totalUsed += val;
        txtTotalStorage.text = $"{totalUsed:0}/{totalCapacity:0}";
    }

    public void OnCloseButtonTap()
    {
        ScreenManager.Close();
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}