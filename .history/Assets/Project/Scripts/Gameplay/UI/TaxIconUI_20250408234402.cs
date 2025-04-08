using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TaxIconUI : MonoBehaviour, IPoolable
{
    [SerializeField] private Button collectButton;
    private TaxBuildingCtrl targetBuilding;

    private Tween scaleTween;

    private void Awake()
    {
        collectButton.onClick.AddListener(OnClickCollect);
    }

    public void Setup(TaxBuildingCtrl building)
    {
        targetBuilding = building;
    }

    private void OnClickCollect()
    {
        if (targetBuilding != null)
        {
            targetBuilding.CollectTax();
        }
    }

    public void OnSpawn()
    {
        transform.localScale = Vector3.zero;
        scaleTween = transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack);
    }

    public void OnDespawn()
    {
        scaleTween?.Kill();
        scaleTween = null;
        targetBuilding = null;
    }
}
