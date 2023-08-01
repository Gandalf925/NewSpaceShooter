using UnityEngine;
using DG.Tweening;

public class BackgroundPanelShrink : MonoBehaviour
{
    public float duration = 1f;
    public float scale = 0.5f;

    private Vector3 originalScale;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalScale = rectTransform.localScale;
    }

    public void Shrink()
    {
        // 左下を起点にして縮小するアンカーポイントを設定する
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;

        // 縮小アニメーション
        rectTransform.DOScale(originalScale * scale, duration);
    }

    public void ResetAnimation()
    {
        // 元のスケールに戻すアニメーション
        rectTransform.DOScale(originalScale, duration).OnComplete(() =>
        {
            // アンカーポイントを元に戻す
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        });
    }
}