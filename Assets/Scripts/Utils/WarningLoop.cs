using UnityEngine;

public class WarningLoop : MonoBehaviour
{
    public float blinkSpeed = 0.8f; // 点滅の速さ
    public float blinkMinAlpha = 0f; // 点滅の最小アルファ値
    public float blinkMaxAlpha = 1f; // 点滅の最大アルファ値

    private CanvasGroup canvasGroup;
    private float timer = 0f;
    private bool isIncreasing = false;

    private void Start()
    {
        // WarningPanelのCanvasGroupを取得する
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        // タイマーを進める
        timer += Time.deltaTime * blinkSpeed;

        // アルファ値を変化させる
        if (isIncreasing)
        {
            canvasGroup.alpha = Mathf.Lerp(blinkMinAlpha, blinkMaxAlpha, timer);
        }
        else
        {
            canvasGroup.alpha = Mathf.Lerp(blinkMaxAlpha, blinkMinAlpha, timer);
        }

        // タイマーが1を超えたら、反転させる
        if (timer > 1f)
        {
            isIncreasing = !isIncreasing;
            timer -= 1f;
        }
    }
}