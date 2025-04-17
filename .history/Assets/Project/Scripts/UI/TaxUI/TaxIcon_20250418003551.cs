using DG.Tweening;
using UnityEngine;

public class TaxIcon : MonoBehaviour
{
    public void Animate()
    {
        transform.DOLocalMoveY(transform.localPosition.y + 10f, 0.5f)
                 .SetLoops(-1, LoopType.Yoyo)
                 .SetEase(Ease.InOutSine)
                 .SetLink(gameObject);
    }
}
