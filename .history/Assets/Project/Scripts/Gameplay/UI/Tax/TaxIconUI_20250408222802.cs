using UnityEngine;
using UnityEngine.UI;

public class TaxIconUI : MonoBehaviour
{
    private TaxBuildingCtrl building;

    public void Setup(TaxBuildingCtrl ctrl)
    {
        building = ctrl;
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        building.CollectTax();
    }
}
