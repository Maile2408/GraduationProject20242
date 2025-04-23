using UnityEngine;

public class TaxBuildingCtrl : MonoBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] private float coinPerCycle = 50f;
    [SerializeField] private float interval = 20f;

    [Header("UI References")]
    [SerializeField] private TaxIcon taxIcon;
    [SerializeField] private TaxText taxText;

    private float timer;
    private bool isReadyToCollect = false;

    public float GetCoinPerCycle() => coinPerCycle;
    public float GetInterval() => interval;

    private void Start()
    {
        if (taxIcon != null) taxIcon.gameObject.SetActive(false);
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
            taxIcon.gameObject.SetActive(true);

            var icon = taxIcon.GetComponent<TaxIcon>();
            icon?.Animate();
        }
    }

    public int Collect()
    {
        if (!isReadyToCollect) return 0;

        isReadyToCollect = false;
        taxIcon?.gameObject.SetActive(false);
        taxText?.Show(coinPerCycle);

        CurrencyManager.Instance.AddCoin((int)coinPerCycle);

        return (int)coinPerCycle;
    }


    public bool IsReadyToCollect() => isReadyToCollect;
}