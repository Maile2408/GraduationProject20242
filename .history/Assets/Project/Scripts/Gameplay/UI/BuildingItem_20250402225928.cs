public class BuildingItem : MonoBehaviour
{
    public Image icon;
    private BuildingInfo info;
    private BuildMenuManager menu;

    public void Setup(BuildingInfo _info, BuildMenuManager _menu)
    {
        info = _info;
        menu = _menu;
        icon.sprite = info.icon;

        GetComponent<Button>().onClick.AddListener(() => menu.ShowPopup(info));
    }
}
