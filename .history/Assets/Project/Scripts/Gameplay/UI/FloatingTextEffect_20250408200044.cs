using TMPro;
using UnityEngine;
using DG.Tweening;

public class FloatingTextEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Play(int amount, Vector3 worldPos)
    {
        text.text = $"+{amount}";
        transform.position = worldPos;

        canvasGroup.alpha = 1f;

        Vector3 targetPos = transform.position + new Vector3(0, 0.5f, 0);

        transform.DOMove(targetPos, 1f);
        canvasGroup.DOFade(0f, 1f).OnComplete(() =>
        {
            PoolManager.Instance.Despawn(gameObject);
        });
    }

    public void OnSpawn()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();

        transform.localScale = Vector3.one;
        canvasGroup.alpha = 1f;
    }

    public void OnDespawn()
    {
        transform.DOKill();
        canvasGroup.alpha = 0f;
    }
}
