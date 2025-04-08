public class TaxBuildingCtrl : SaiBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] private float taxCycle = 30f;
    [SerializeField] private float taxAmount = 100f;

    private float timer = 0f;
    private bool taxReady = false;

    public bool IsTaxReady => taxReady;
    public float TaxAmount => taxAmount;

    private void Update()
    {
        if (taxReady) return;

        timer += Time.deltaTime;

        if (timer >= taxCycle)
        {
            taxReady = true;
            timer = taxCycle; 

            TaxUIManager.Instance.ShowTaxIcon(this, transform.position);
        }
    }

    public void ResetTaxTimer()
    {
        timer = 0f;
        taxReady = false;
    }
}
