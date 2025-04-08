using UnityEngine;
using DG.Tweening;

public class TaxCoinEffect : MonoBehaviour, IPoolable
{
    private static RectTransform targetUI;

    public static void SetTargetUI(RectTransform uiTarget)
    {
        targetUI = uiTarget;
    }

    public static void Spawn(Vector3 worldPos, int coinCount = 6)
    {
        for (int i = 0; i < coinCount; i++)
        {
            GameObject coin = PoolManager.Instance.Spawn(PoolPrefabPath.FX("FX_Coin"));
            coin.transform.position = worldPos;
            coin.GetComponent<TaxCoinEffect>()?.FlyToUI(i * 0.05f);
        }
    }

    public void FlyToUI(float delay)
    {
        if (targetUI == null || Camera.main == null)
        {
            PoolManager.Instance.Despawn(gameObject);
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector2 targetAnchoredPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            targetUI.parent as RectTransform,
            targetUI.position,
            Camera.main,
            out targetAnchoredPos
        );

        Vector3 worldTarget = Camera.main.ScreenToWorldPoint(new Vector3(
            targetUI.position.x, targetUI.position.y, screenPos.z
        ));

        transform.DOMove(worldTarget, 0.8f)
            .SetDelay(delay)
            .SetEase(Ease.InOutSine)
            .OnComplete(() => PoolManager.Instance.Despawn(gameObject));
    }

    public void OnSpawn()
    {
        transform.localScale = Vector3.one;
    }

    public void OnDespawn()
    {
        transform.DOKill();
    }
}
