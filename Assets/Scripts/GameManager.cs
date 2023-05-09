using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    PlayerController player;
    public int lives = 3;
    public int powerupPoint { get; private set; } = 0;

    int startingScore = 0; // スタートするポイント
    public int score = 0; // 表示する最大のポイント
    [SerializeField] TMP_Text scoreText; // ポイントを表示するTextオブジェクト

    private int currentScore;
    public int powerupCount;
    EnemySpawnerStage1 enemySpawnerStage1;
    UIManager uIManager;

    void Start()
    {
        // スタート時にポイントを設定
        currentScore = startingScore;
        scoreText.text = currentScore.ToString();
        player = FindObjectOfType<PlayerController>();
        enemySpawnerStage1 = FindObjectOfType<EnemySpawnerStage1>();
        uIManager = FindObjectOfType<UIManager>();
        UpdateLives(0);
    }

    public void UpdateLives(int livesDelta)
    {
        lives += livesDelta;
        for (int i = 0; i < uIManager.lifeObjects.Length; i++)
        {
            if (i < lives)
            {
                uIManager.lifeObjects[i].SetActive(true);
            }
            else
            {
                uIManager.lifeObjects[i].SetActive(false);
            }
        }

        if (lives <= 0)
        {
            StartCoroutine(GameOver());
        }
    }

    public void UpdateScore(int point)
    {
        score += (point * 100);

        scoreText.text = score.ToString();


        // 50ポイント毎にパワーアップする（最大値:5）
        if (powerupPoint >= 50 && powerupCount < 5)
        {
            player.Powerup();
            ResetPowerupPoint();
        }
    }

    IEnumerator GameOver()
    {
        Debug.Log("GameOver");
        uIManager.blackoutPanel.DOFade(0.5f, 1f);
        yield return new WaitForSecondsRealtime(3f);

        uIManager.blackoutPanel.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(2f);

        RestartScene();
    }

    private void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
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
