using UnityEngine;

public class TaxBuildingCtrl : MonoBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] private float taxCycle = 30f;
    [SerializeField] private float taxAmount = 100;

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
            Debug.Log("Call ShowTaxIcon");
            TaxUIManager.Instance.ShowTaxIcon(this, transform.position);
        }
    }

    public void CollectTax()
    {
        if (!taxReady) return;

        CurrencyManager.Add(taxAmount);
        timer = 0f;
        taxReady = false;

        TaxUIManager.Instance.HideTaxIcon(this);
        TaxCoinEffect.Spawn(transform.position, 6);
        FloatingTextSpawner.Show($"+{taxAmount}", transform.position + Vector3.up * 2f);
    }
}
