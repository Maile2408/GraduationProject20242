using UnityEngine;

public class TaxBuildingCtrl : MonoBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] float coinPerCycle = 50f;
    [SerializeField] float interval = 20f;

    [Header("UI References")]
    [SerializeField] TaxIcon taxIcon;
    [SerializeField] TaxText taxText;

    [SerializeField] float timer;
    [SerializeField] bool isReadyToCollect = false;

    public float GetCoinPerCycle() => coinPerCycle;
    public float GetInterval() => interval;
    public float GetCurrentTimer() => timer;


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

    public void RestoreState(bool ready, float savedTimer)
    {
        isReadyToCollect = ready;
        timer = savedTimer;

        if (taxIcon != null) taxIcon.gameObject.SetActive(ready);
        if (taxText != null) taxText.gameObject.SetActive(false);

        if (ready && taxIcon != null)
        {
            var icon = taxIcon.GetComponent<TaxIcon>();
            icon?.Animate();
        }
    }

    public bool IsReadyToCollect() => isReadyToCollect;
}