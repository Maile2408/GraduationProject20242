using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Globalization;

public class BuildingInfoUIController : MonoBehaviour, IKeyBack
{
    public const string NAME = "BuildingInfoUI";

    [Header("UI Elements")]
    [SerializeField] private GameObject panel;
    [SerializeField] private TextMeshProUGUI buildingNameText;
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI workerText;
    [SerializeField] private List<TextMeshProUGUI> resourceTexts;
    [SerializeField] private TextMeshProUGUI taxText;
    [SerializeField] private TextMeshProUGUI intervalText;

    public static BuildingCtrl pendingBuilding;

    private void OnEnable()
    {
        if (pendingBuilding != null)
        {
            Show(pendingBuilding);
            pendingBuilding = null;
        }
    }

    public void OnCloseButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
    }

    public void Show(BuildingCtrl building)
    {
        panel.SetActive(true);
        ClearUI();

        // Building Name
        buildingNameText.text = building.buildingInfo.buildingName;

        // Building Type
        typeText.text = $"Type: {building.buildingInfo.category}";

        // Workers
        var taskType = building.buildingTaskType;
        bool showWorker = taskType == BuildingTaskType.home || taskType == BuildingTaskType.workStation;
        workerText.gameObject.SetActive(showWorker);

        if (showWorker && building.workers != null)
        {
            int cur = building.workers.WorkerCount();
            int max = building.workers.MaxWorker();
            workerText.text = $"Workers: {cur}/{max}";
        }

        // Inventory
        foreach (var text in resourceTexts)
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }

        if (taskType != BuildingTaskType.none && building.warehouse != null)
        {
            if (building.warehouse is WarehouseWH wh)
            {
                float total = 0f;
                foreach (var res in wh.GetStockedResources())
                    total += res.Current();

                float cap = wh.GetStorageCapacity();
                if (resourceTexts.Count > 0)
                {
                    resourceTexts[0].gameObject.SetActive(true);
                    resourceTexts[0].text = $"Storage: {total}/{cap}";
                }
            }
            else
            {
                var list = building.warehouse.GetStockedResources();
                for (int i = 0; i < list.Count && i < resourceTexts.Count; i++)
                {
                    var res = list[i];
                    resourceTexts[i].gameObject.SetActive(true);
                    resourceTexts[i].text = $"{ToTitleCase(res.Name().ToString())}: {res.Current()}/{res.Max()}";
                }
            }
        }

        // Tax Info
        var tax = building.GetComponent<TaxBuildingCtrl>();
        bool hasTax = tax != null;
        taxText.gameObject.SetActive(hasTax);
        intervalText.gameObject.SetActive(hasTax);

        if (hasTax)
        {
            taxText.text = $"Tax: {tax.GetCoinPerCycle()} coin";
            intervalText.text = $"Interval: {tax.GetInterval()}s";
        }
    }

    private void ClearUI()
    {
        buildingNameText.text = "";
        typeText.text = "";
        workerText.text = "";
        taxText.text = "";
        intervalText.text = "";

        foreach (var text in resourceTexts)
        {
            text.text = "";
            text.gameObject.SetActive(false);
        }
    }

    private string ToTitleCase(string input)
    {
        if (string.IsNullOrEmpty(input)) return "";
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(input.ToLower());
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}
