using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    public int lives = 3;
    public int powerupPoint { get; private set; } = 0;

    int startingScore = 0; // スタートするポイント
    public int score = 0; // 表示する最大のポイント
    public float duration = 1f; // アニメーションにかける時間
    [SerializeField] TMP_Text scoreText; // ポイントを表示するTextオブジェクト

    private int currentScore;

    void Start()
    {
        // スタート時にポイントを設定
        currentScore = startingScore;
        scoreText.text = currentScore.ToString();
    }

    public void UpdateLives()
    {
        lives -= 1;
    }

    public void UpdateScore(int point)
    {
        score += (point * 100);

        // // DOTweenを使って値を徐々に変化させる
        // DOTween.To(() => currentScore, x => currentScore = x, currentScore + 1, duration)
        //     .OnUpdate(() =>
        //     {
        //         // テキストを更新
        //         scoreText.text = currentScore.ToString();
        //     })
        //     .OnComplete(() =>
        //     {
        //         // endPointに到達したら、アニメーションを停止する
        //         if (currentScore == score)
        //         {
        //             DOTween.Kill(this);
        //         }
        //     });

        scoreText.text = score.ToString();
    }

    public void AddPowerupPoint(int point)
    {
        powerupPoint += point;
    }
    public void ResetPowerupPoint()
    {
        powerupPoint = 0;
    }
}
