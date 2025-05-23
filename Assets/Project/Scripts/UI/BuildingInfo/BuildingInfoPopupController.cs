using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInfoPopupController : MonoBehaviour, IKeyBack
{
    public const string NAME = "BuildingInfoPopup";

    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI buildingName;
    [SerializeField] TextMeshProUGUI description;

    [SerializeField] private UnityEngine.GameObject coinIcon;
    [SerializeField] private TextMeshProUGUI coinAmount;

    [SerializeField] private UnityEngine.GameObject logwoodIcon;
    [SerializeField] private TextMeshProUGUI logwoodAmount;

    [SerializeField] private UnityEngine.GameObject plankIcon;
    [SerializeField] private TextMeshProUGUI plankAmount;

    [SerializeField] private UnityEngine.GameObject stoneIcon;
    [SerializeField] private TextMeshProUGUI stoneAmount;

    [SerializeField] private Button buildButton;
    [SerializeField] private UnityEngine.GameObject warningPanel;
    [SerializeField] private TextMeshProUGUI warningText;

    public static BuildingInfo pendingInfo;
    BuildingInfo currentInfo;

    private void OnEnable()
    {
        if (pendingInfo != null)
        {
            currentInfo = pendingInfo;
            Setup(pendingInfo);
            pendingInfo = null;
        }
    }

    public void Setup(BuildingInfo info)
    {
        if (icon != null) icon.sprite = info.icon;
        if (buildingName != null) buildingName.text = info.buildingName;
        if (description != null) description.text = info.description;

        if (coinIcon != null) coinAmount.text = ((int)info.coin).ToString();

        ShowResource(logwoodIcon, logwoodAmount, info, ResourceName.logwood);
        ShowResource(plankIcon, plankAmount, info, ResourceName.plank);
        ShowResource(stoneIcon, stoneAmount, info, ResourceName.stone);
    }

    private void ShowResource(UnityEngine.GameObject icon, TextMeshProUGUI text, BuildingInfo info, ResourceName name)
    {
        if (icon == null || text == null) return;

        float amount = info.cost.FirstOrDefault(r => r.name == name)?.number ?? 0;

        bool active = amount > 0;
        icon.SetActive(active);
        text.gameObject.SetActive(active);
        if (active) text.text = amount.ToString();
    }

    public void OnCloseButtonTap()
    {
        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
        ScreenManager.Add<BuildMenuController>(BuildMenuController.NAME);
    }

    public void OnBuildButtonTap()
    {
        if (!CanBuild())
        {
            warningPanel.SetActive(true);
            return;
        }

        AudioManager.Instance.PlayButtonTap();
        ScreenManager.Close();
        BuildManager.Instance.PrepareToBuild(currentInfo);
    }

    private bool CanBuild()
    {
        if (currentInfo == null) return false;

        bool hasCoin = CurrencyManager.Instance.Coin >= currentInfo.coin;
        bool hasRes = StorageResourceManager.Instance.HasEnoughResources(currentInfo.cost);

        if (!hasCoin && !hasRes)
            warningText.text = "[Not enough coin and resources!]";
        else if (!hasCoin)
            warningText.text = "[Not enough coin!]";
        else if (!hasRes)
            warningText.text = "[Not enough resources!]";

        return hasCoin && hasRes;
    }

    public void OnKeyBack()
    {
        ScreenManager.Close();
    }
}