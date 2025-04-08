using UnityEngine;

public class TaxBuildingCtrl : MonoBehaviour
{
    [Header("Tax Settings")]
    [SerializeField] private float taxCycle = 30f;
    [SerializeField] private int taxAmount = 100;
    [SerializeField] private GameObject taxIconPrefab;

    private float timer = 0f;
    private bool taxReady = false;
    private GameObject taxIconInstance;

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

    private void ShowTaxIcon()
    {
        if (taxIconInstance == null)
        {
            taxIconInstance = Instantiate(taxIconPrefab, transform);
            taxIconInstance.transform.localPosition = new Vector3(0, 3f, 0);
            taxIconInstance.GetComponent<TaxIcon>()?.Setup(this);
        }

        taxIconInstance.SetActive(true);
    }

    private void HideTaxIcon()
    {
        if (taxIconInstance != null)
            taxIconInstance.SetActive(false);
    }

    public void CollectTax()
    {
        if (!taxReady) return;

        CurrencyManager.Add(taxAmount);
        timer = 0f;
        taxReady = false;

        HideTaxIcon();

        TaxCoinEffect.Spawn(transform.position, 6);
        ShowPlusCoinText(taxAmount);
    }

    private void ShowPlusCoinText(int amount)
    {
        if (Camera.main == null) return;

        GameObject fx = Instantiate(Resources.Load<GameObject>("FX/FX_CoinText"));
        var effect = fx.GetComponent<FloatingTextEffect>();

        Vector3 uiWorldPos = Camera.main.ScreenToWorldPoint(Camera.main.WorldToScreenPoint(transform.position) + new Vector3(0, 30f, 0));
        fx.transform.SetParent(GameObject.Find("Canvas").transform, false); // hoặc HUD canvas bạn dùng
        effect.Play(amount, uiWorldPos);
    }

}
