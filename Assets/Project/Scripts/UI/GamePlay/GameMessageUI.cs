using UnityEngine;
using TMPro;
using DG.Tweening;

public enum GameMessageType { Info, Success, Warning, Guide }

public class GameMessageUI : MonoBehaviour, IPoolable
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private CanvasGroup canvasGroup;

    [Header("Tween Settings")]
    [SerializeField] private float slideInX = 300f;
    [SerializeField] private float slideOutX = -300f; 
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float displayDuration = 2.5f;

    private Sequence messageSequence;

    public void ShowMessage(string message, GameMessageType type)
    {
        messageText.text = message;
        messageText.color = GetColor(type);

        messageSequence?.Kill();
        ResetState();

        messageSequence = DOTween.Sequence().SetLink(gameObject)
            .AppendCallback(() =>
            {
                canvasGroup.alpha = 0f;
                panel.anchoredPosition = new Vector2(slideOutX, panel.anchoredPosition.y);
            })
            .Append(canvasGroup.DOFade(1f, 0.2f))
            .Join(panel.DOAnchorPosX(slideInX, slideDuration).SetEase(Ease.OutBack))
            .AppendInterval(displayDuration)
            .Append(canvasGroup.DOFade(0f, 0.4f))
            .OnComplete(() =>
            {
                PoolManager.Instance.Despawn(gameObject);
            });
    }

    private void ResetState()
    {
        panel.anchoredPosition = new Vector2(slideOutX, panel.anchoredPosition.y);
        canvasGroup.alpha = 0f;
    }

    public void OnSpawn()
    {
        ResetState();
    }

    public void OnDespawn()
    {
        messageSequence?.Kill();
        ResetState();
    }

    private Color GetColor(GameMessageType type)
    {
        return type switch
        {
            GameMessageType.Warning => Color.red,
            GameMessageType.Success => Color.green,
            GameMessageType.Guide => Color.cyan,
            _ => Color.white,
        };
    }
}
