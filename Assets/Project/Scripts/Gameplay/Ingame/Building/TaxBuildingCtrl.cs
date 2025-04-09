using UnityEngine;

public class TaxBuildingCtrl : MonoBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] private float coinPerCycle = 50f;
    [SerializeField] private float interval = 20f;

    [Header("UI References")]
    [SerializeField] private GameObject taxIcon;
    [SerializeField] private TaxText taxText;

    private float timer;
    private bool isReadyToCollect = false;

    private void Start()
    {
        if (taxIcon != null) taxIcon.SetActive(false);
        if (taxText != null) taxText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isReadyToCollect) return;

        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;
            MarkReady();
        }
    }

    private void MarkReady()
    {
        isReadyToCollect = true;

        if (taxIcon != null)
        {
            taxIcon.SetActive(true);

            var icon = taxIcon.GetComponent<TaxIcon>();
            icon?.Animate();
        }
    }

    public void Collect()
    {
        if (!isReadyToCollect) return;

        isReadyToCollect = false;
        taxIcon?.SetActive(false);
        taxText?.Show(coinPerCycle);

        CurrencyManager.Instance.AddCoin(coinPerCycle);
    }

    public bool IsReadyToCollect() => isReadyToCollect;
}
