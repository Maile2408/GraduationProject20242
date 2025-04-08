using DG.Tweening;
using UnityEngine;

public class TaxBuildingCtrl : MonoBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] private float taxCycle = 60f;
    [SerializeField] private float taxAmount = 50;
    private Vector3 initialPos;

    private float timer = 0f;
    private bool taxReady = false;

    [SerializeField] private GameObject taxIcon;

    private void Start()
    {
        initialPos = transform.localPosition;
        StartBounce();
    }

    private void Update()
    {
        if (taxReady) return;

        timer += Time.deltaTime;
        if (timer >= taxCycle)
        {
            taxReady = true;
            ShowTaxIcon();
        }
    }

    private void StartBounce()
    {
        transform.DOLocalMoveY(initialPos.y + 0.3f, 0.4f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine);
    }

    private void ShowTaxIcon()
    {
        if (taxIcon != null)
            taxIcon.SetActive(true);
    }

    private void HideTaxIcon()
    {
        if (taxIcon != null)
            taxIcon.SetActive(false);
    }

    public void CollectTax()
    {
        if (!taxReady) return;

        CurrencyManager.Add(taxAmount);
        timer = 0f;
        taxReady = false;

        HideTaxIcon();
    }
}
