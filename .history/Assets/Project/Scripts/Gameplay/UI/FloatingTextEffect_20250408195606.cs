using TMPro;
using UnityEngine;
using DG.Tweening;

public class FloatingTextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void Play(int amount, Vector3 worldPos)
    {
        if (text == null) text = GetComponentInChildren<TextMeshProUGUI>();

        text.text = $"+{amount}";
        transform.position = worldPos;

        CanvasGroup group = GetComponent<CanvasGroup>();
        if (group == null) group = gameObject.AddComponent<CanvasGroup>();
        group.alpha = 1f;

        Vector3 targetPos = transform.position + new Vector3(0, 0.5f, 0);

        transform.DOMove(targetPos, 1f);
        group.DOFade(0f, 1f).OnComplete(() => Destroy(gameObject));
    }
}
