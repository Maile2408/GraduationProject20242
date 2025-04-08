using TMPro;
using UnityEngine;
using DG.Tweening;

public class FloatingTextEffect : MonoBehaviour, IPoolable
{
    [SerializeField] private TextMeshProUGUI text;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();

        if (text == null)
            text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void PlayText(string value)
    {
        text.text = value;
        canvasGroup.alpha = 1f;

        Vector3 startPos = transform.localPosition;
        Vector3 targetPos = startPos + new Vector3(0, 50f, 0);

        transform.localPosition = startPos;
        transform.DOLocalMove(targetPos, 1f).SetEase(Ease.OutQuad);
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
