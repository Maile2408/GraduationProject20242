public class BuildMenuManager : MonoBehaviour
{
    [Header("Category Buttons")]
    public List<Button> categoryButtons;
    public List<CategoryButtonGroup> categoryGroups;

    [Header("Building List")]
    public Transform contentParent;
    public GameObject buildingItemPrefab;
    public List<BuildingInfo> allBuildings;

    [Header("Popup")]
    public GameObject popup;
    public Image popupIcon;
    public TMP_Text popupName, popupDesc, popupCost;

    private BuildingCategory currentCategory;
    private BuildingInfo selectedInfo;

    private void Start()
    {
        ShowCategory(BuildingCategory.Needs); // mặc định chọn Needs
    }

    public void ShowCategory(BuildingCategory category)
    {
        currentCategory = category;

        // Clear item cũ
        foreach (Transform child in contentParent) Destroy(child.gameObject);

        // Tạo item mới
        foreach (var info in allBuildings.Where(b => b.category == category))
        {
            var go = Instantiate(buildingItemPrefab, contentParent);
            go.GetComponent<BuildingItemUI>().Setup(info, this);
        }

        // Cập nhật màu alpha cho nút
        foreach (var group in categoryGroups)
        {
            var img = group.GetComponent<Image>();
            Color c = img.color;
            c.a = (group.category == category) ? 1f : 0.2f;
            img.color = c;
        }
    }

    public void ShowPopup(BuildingInfo info)
    {
        selectedInfo = info;
        popup.SetActive(true);
        popupIcon.sprite = info.icon;
        popupName.text = info.buildingName;
        popupDesc.text = info.description;
        popupCost.text = $"Gold: {info.costGold}, Wood: {info.costWood}, Stone: {info.costStone}";
    }

    public void OnClickBuild()
    {
        if (selectedInfo != null)
        {
            BuildingPlacer.Instance.StartPlacing(selectedInfo.prefab); // bạn tự code hệ thống này
            popup.SetActive(false);
        }
    }
}
