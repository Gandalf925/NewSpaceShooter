using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;

public class OpeningSceneManager : MonoBehaviour
{
    public string gameSceneName = "Stage1";
    public VideoPlayer videoPlayer;
    public GameObject loadingPanel;
    public GameObject blackoutPanel;
    public float fadeOutDuration = 1f;
    public KeyCode skipKey = KeyCode.Space;

    public TMP_Text pushSpaceKeytext;
    public float blinkInterval = 0.5f;

    bool isReadyToLoadScene = false;
    bool isVideoPlaying = false;
    bool canBlink = true;

    void Start()
    {
        // ロード画面を表示する
        loadingPanel.SetActive(true);
        blackoutPanel.SetActive(false);

        // 動画の再生が終わったらフラグをtrueにする
        videoPlayer.loopPointReached += EndReached;

        // 動画の再生を開始する
        videoPlayer.Play();
    }

    void Update()
    {
        isVideoPlaying = videoPlayer.isPlaying;

        if (isVideoPlaying)
        {
            loadingPanel.SetActive(false);


            if (canBlink)
            {
                BlinkText();
                canBlink = false;
            }
        }

        if (Input.GetKeyDown(skipKey))
        {
            if (isReadyToLoadScene)
            {
                EndReached(videoPlayer);
            }
            else
            {
                // フェードアウトのアニメーションを実行する
                EndReached(videoPlayer);
            }
        }
    }

    private void EndReached(UnityEngine.Video.VideoPlayer vp)
    {
        isReadyToLoadScene = true;
        blackoutPanel.SetActive(true);
        // フェードアウトのアニメーションを実行する
        DOTween.To(() => videoPlayer.targetCameraAlpha, (x) => videoPlayer.targetCameraAlpha = x, 0f, fadeOutDuration).OnComplete(LoadGameScene);
    }

    private void LoadGameScene()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    void BlinkText()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(pushSpaceKeytext.DOFade(0, blinkInterval));
        sequence.Append(pushSpaceKeytext.DOFade(1, blinkInterval));
        sequence.SetLoops(-1).OnComplete(() => canBlink = true);
    }
}