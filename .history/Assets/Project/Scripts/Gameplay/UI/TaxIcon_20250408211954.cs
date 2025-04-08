using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TaxIcon : MonoBehaviour, IPointerClickHandler
{
    private TaxBuildingCtrl taxCtrl;

    [SerializeField] private float bounceHeight = 0.3f;
    [SerializeField] private float bounceTime = 0.4f;

    private Vector3 initPos;

    private void Start()
    {
        SetupCanvasCamera();
        SetupUIClick();
        StartBounce();
    }

    public void Setup(TaxBuildingCtrl ctrl)
    {
        this.taxCtrl = ctrl;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked via IPointerClickHandler!");
        taxCtrl?.CollectTax();
    }

    private void SetupUIClick()
    {
        Button btn = GetComponentInChildren<Button>();
        if (btn != null)
        {
            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(OnClick);
        }

        Graphic graphic = GetComponentInChildren<Graphic>();
        if (graphic != null)
        {
            graphic.raycastTarget = true;
        }
    }

    public void OnClick()
    {
        Debug.Log("TaxIcon clicked via Button");
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
            canvas.sortingOrder = 98;
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
