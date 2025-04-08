using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    [SerializeField] private float coin = 0;
    public UnityEvent<float> onCoinChanged;

    private void Awake()
    {
        Instance = this;
    }

    public static void Add(float amount)
    {
        Instance.coin += amount;
        Instance.onCoinChanged?.Invoke(Instance.coin);
    }

    public static float Get() => Instance.coin;
}
