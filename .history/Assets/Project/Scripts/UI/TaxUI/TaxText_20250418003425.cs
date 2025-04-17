using DG.Tweening;
using UnityEngine;

public class TaxText : MonoBehaviour
{
    [SerializeField] private TMPro.TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Animation Settings")]
    [SerializeField] private float flyDistance = 100f;
    [SerializeField] private float duration = 2f;

    public void Show(float amount)
    {
        gameObject.SetActive(true);
        text.text = $"+{amount}";
        canvasGroup.alpha = 1;

        var rect = GetComponent<RectTransform>();
        Vector3 startPos = rect.localPosition;
        rect.localPosition = startPos;

        rect.DOLocalMoveY(startPos.y + flyDistance, duration)
            .SetEase(Ease.OutCubic)
            .SetLink(gameObject);

        canvasGroup.DOFade(0, duration)
            .SetLink(gameObject)
            .OnComplete(() =>
            {
                gameObject.SetActive(false);
                rect.localPosition = startPos;
                canvasGroup.alpha = 1;
            });

    }
}
