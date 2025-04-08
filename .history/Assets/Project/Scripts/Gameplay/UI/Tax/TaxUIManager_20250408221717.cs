using UnityEngine;
using System.Collections.Generic;

public class TaxUIManager : MonoBehaviour
{
    public static TaxUIManager Instance;

    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private GameObject taxIconUIPrefab;

    private Dictionary<TaxBuildingCtrl, GameObject> iconInstances = new();

    private void Awake()
    {
        Debug.Log("TaxUIManager.Awake()");
        Instance = this;
    }

    public void ShowTaxIcon(TaxBuildingCtrl building, Vector3 worldPos)
    {
        if (iconInstances.ContainsKey(building)) return;

        Vector2 anchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, Camera.main.WorldToScreenPoint(worldPos), Camera.main, out anchoredPos);

        GameObject icon = Instantiate(taxIconUIPrefab, canvasRect);
        icon.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
        icon.GetComponent<TaxIconUI>().Setup(building);

        iconInstances.Add(building, icon);
    }

    public void HideTaxIcon(TaxBuildingCtrl building)
    {
        if (iconInstances.TryGetValue(building, out GameObject icon))
        {
            Destroy(icon);
            iconInstances.Remove(building);
        }
    }
}
