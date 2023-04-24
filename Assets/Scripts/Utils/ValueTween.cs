using UnityEngine;
using DG.Tweening;

public class ValueTween : MonoBehaviour
{
    public float startValue = 0f;
    public float endValue = 1f;
    public float duration = 1f;

    private float currentValue;

    void Start()
    {
        // Tweenの作成
        var valueTween = DOTween.To(() => startValue, x => currentValue = x, endValue, duration);

        // Tweenの設定
        valueTween.SetEase(Ease.Linear);

        // Tweenの再生
        valueTween.Play();
    }
}