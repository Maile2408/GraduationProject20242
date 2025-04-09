using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Collections;

public enum GameMessageType { Info, Success, Warning, Guide }

public class GameMessageUI : MonoBehaviour
{
    [SerializeField] private RectTransform panel;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Tween Settings")]
    [SerializeField] private float slideInX = 300f;
    [SerializeField] private float slideOutX = -300f;
    [SerializeField] private float slideDuration = 0.5f;
    [SerializeField] private float displayDuration = 3f;

    private Sequence messageSequence;

    private void Awake()
    {
        panel.anchoredPosition = new Vector2(slideOutX, panel.anchoredPosition.y);
    }

    public IEnumerator ShowMessage(string message, GameMessageType type)
    {
        messageText.text = message;
        messageText.color = GetColor(type);

        messageSequence?.Kill();
        panel.anchoredPosition = new Vector2(slideOutX, panel.anchoredPosition.y);

        messageSequence = DOTween.Sequence()
            .Append(panel.DOAnchorPosX(slideInX, slideDuration).SetEase(Ease.OutBack))
            .AppendInterval(displayDuration)
            .Append(panel.DOAnchorPosX(slideOutX, slideDuration).SetEase(Ease.InBack));

        yield return messageSequence.WaitForCompletion();
    }


    private Color GetColor(GameMessageType type)
    {
        return type switch
        {
            GameMessageType.Warning => Color.red,
            GameMessageType.Success => Color.green,
            GameMessageType.Info => Color.white,
            GameMessageType.Guide => new Color(1f, 0.85f, 0.3f),
            _ => Color.white,
        };
    }
}
