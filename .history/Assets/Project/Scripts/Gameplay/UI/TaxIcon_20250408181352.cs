using UnityEngine;

public class TaxIcon : MonoBehaviour
{
    private TaxBuildingCtrl taxCtrl;

    private void Awake()
    {
        SetupCanvasCamera();
    }

    public void Setup(TaxBuildingCtrl ctrl)
    {
        taxCtrl = ctrl;
    }

    public void OnClick()
    {
        if (taxCtrl == null) return;

        taxCtrl.CollectTax();
        TaxCoinEffect.Spawn(transform.position, 6);
    }

    private void SetupCanvasCamera()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            var cam = GodModeCtrl.Instance?._camera;
            if (cam != null) canvas.worldCamera = cam;
        }
    }
}
