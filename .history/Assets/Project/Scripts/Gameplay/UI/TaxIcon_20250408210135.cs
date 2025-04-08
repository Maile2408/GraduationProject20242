using UnityEngine;
using DG.Tweening;

public class TaxIcon : MonoBehaviour
{
    private TaxBuildingCtrl taxCtrl;

    [SerializeField] private float bounceHeight = 0.3f;
    [SerializeField] private float bounceTime = 0.4f;

    private Vector3 initPos;

    private void Start()
    {
        SetupCanvasCamera();
        StartBounce();
    }

    public void Setup(TaxBuildingCtrl ctrl)
    {
        this.taxCtrl = ctrl;
    }

    public void OnClick()
    {
        Debug.Log("TaxIcon clicked");
        taxCtrl?.CollectTax();
    }

    private void SetupCanvasCamera()
    {
        Canvas canvas = GetComponentInChildren<Canvas>();
        if (canvas != null && canvas.renderMode == RenderMode.WorldSpace)
        {
            var cam = GodModeCtrl.Instance?._camera ?? Camera.main;
            if (cam != null) canvas.worldCamera = cam;

            canvas.overrideSorting = true;
            canvas.sortingOrder = 1000; 
        }
    }

    private void StartBounce()
    {
        initPos = transform.localPosition;
        transform.DOLocalMoveY(initPos.y + bounceHeight, bounceTime)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }
}
