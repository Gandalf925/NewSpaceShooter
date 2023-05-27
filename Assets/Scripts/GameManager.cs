using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public SoundManager soundManager;
    bool isPaused;
    public Button fullscreenButton;

    void Start()
    {
        // スタート時にポイントを設定
        currentScore = startingScore;
        scoreText.text = currentScore.ToString();
        player = FindObjectOfType<PlayerController>();
        enemySpawnerStage1 = FindObjectOfType<EnemySpawnerStage1>();
        uIManager = FindObjectOfType<UIManager>();

        // スマホ画面の場合にのみボタンを表示する
        if (IsMobileScreen())
        {
            fullscreenButton.gameObject.SetActive(true);
        }
        else
        {
            fullscreenButton.gameObject.SetActive(false);
        }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))  // 例としてEscapeキーを押すと一時停止するようにしています
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            uIManager.pauseButtonIcon.sprite = uIManager.playbackImage;
            Time.timeScale = 0f;  // ゲームの時間を停止させる
            // 他の一時停止に関連する処理を実行する（BGM停止、ポーズメニューの表示など）
            SoundManager.instance.PauseBGM();
        }
        else
        {
            uIManager.pauseButtonIcon.sprite = uIManager.pauseImage;
            Time.timeScale = 1f;  // ゲームの時間を再開させる
            // 他の一時停止解除に関連する処理を実行する（BGM再生、ポーズメニューの非表示など）
            SoundManager.instance.ResumeBGM();
        }
    }


    private bool IsMobileScreen()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // スマホ画面の条件を判定する（例: 幅が400未満、高さが600未満）
        if (screenWidth < 400 && screenHeight < 600)
        {
            return true;
        }

        return false;
    }
    public void ToggleFullscreen()
    {
        if (Screen.fullScreen)
        {
            // スマートフォンの解像度を取得し、フルスクリーンモードに切り替える
            Resolution currentResolution = Screen.currentResolution;
            Screen.SetResolution(currentResolution.width, currentResolution.height, true);  // スマートフォンの解像度を設定
        }
    }
}
