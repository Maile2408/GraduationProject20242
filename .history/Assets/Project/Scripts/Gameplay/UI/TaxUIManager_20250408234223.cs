using System.Collections.Generic;
using UnityEngine;

public class TaxUIManager : MonoBehaviour
{
    public static TaxUIManager Instance;

    [Header("Canvas Settings")]
    [SerializeField] private RectTransform canvasRect;
    [SerializeField] private Camera uiCamera;

    private readonly string taxIconPoolPath = PoolPrefabPath.UI("TaxIconUI");
    private readonly Dictionary<TaxBuildingCtrl, TaxIconUI> iconMap = new();

    private void Awake()
    {
        Instance = this;
    }

    public void ShowTaxIcon(TaxBuildingCtrl building, Vector3 worldPos)
    {
        if (iconMap.ContainsKey(building)) return;

        Vector2 screenPos = uiCamera.WorldToScreenPoint(worldPos + Vector3.up * 2f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out Vector2 anchoredPos);

        GameObject iconGO = PoolManager.Instance.Spawn(taxIconPoolPath, canvasRect);
        RectTransform rt = iconGO.GetComponent<RectTransform>();
        rt.anchoredPosition = anchoredPos;

        TaxIconUI icon = iconGO.GetComponent<TaxIconUI>();
        icon.Setup(building);

        iconMap[building] = icon;
    }

    public void HideTaxIcon(TaxBuildingCtrl building)
    {
        if (!iconMap.TryGetValue(building, out var icon)) return;

        PoolManager.Instance.Despawn(icon.gameObject);
        iconMap.Remove(building);
    }

    public void UpdateIconPosition(TaxBuildingCtrl building)
    {
        if (!iconMap.TryGetValue(building, out var icon)) return;

        Vector2 screenPos = uiCamera.WorldToScreenPoint(building.transform.position + Vector3.up * 2f);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, uiCamera, out Vector2 anchoredPos);

        icon.GetComponent<RectTransform>().anchoredPosition = anchoredPos;
    }

    public void SetUICamera(Camera cam)
    {
        uiCamera = cam;
    }
}
