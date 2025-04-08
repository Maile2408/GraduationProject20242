using UnityEngine;

public static class CurrencyManager
{
    private static int totalCoin = 0;

    public static int TotalCoin => totalCoin;

    public static void Add(float amount)
    {
        totalCoin += Mathf.RoundToInt(amount);
        Debug.Log($"[CurrencyManager] Add +{amount}, total = {totalCoin}");

        GamePlayController.Instance?.UpdateCoinDisplay();
    }

    public static void Reset()
    {
        totalCoin = 0;
        GamePlayController.Instance?.UpdateCoinDisplay();
    }
}
