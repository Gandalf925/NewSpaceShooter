using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Stage2Manager : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;

    public float scrollSpeed = 5f;  // ステージのスクロール速度
    public float bossScrollSpeed = 2f;  // ボス戦時のスクロール速度
    public float bossDelayTime = 2f;  // ボス戦開始前の遅延時間
    public float bossReverseDelayTime = 1f;  // ボス戦開始後の逆向きスクロール開始までの遅延時間

    public bool isBossBattle = false;  // ボス戦が開始されたかどうか
    private bool isReverseScroll = false;  // 逆向きスクロールが開始されたかどうか
    private bool isScrollStopped = false;  // スクロールが一時停止中かどうか

    private float initialScrollSpeed;  // 初期のスクロール速度
    public GameObject walls;  // 画面上下の壁オブジェクト

    public GameObject warningPanel;
    public Transform playerStayPos;


    public Transform bossStartPos;

    public GameObject bossPrefab;

    public GameObject bossDecoy;

    GameObject boss;


    public GameObject backgroundPanel;
    [SerializeField] GameObject startTextFrame;
    [SerializeField] Transform frameStartPos;
    [SerializeField] Transform frameStopPos;
    [SerializeField] Transform frameEndPos;
    UIManager uIManager;

    bool isBossAppeared;

    [Header("BGM")]
    public AudioClip stage2BGM;
    public AudioClip stage2BossBGM;

    private void Start()
    {
        BGMManager.instance.PlayBGM(stage2BGM);
        player = FindObjectOfType<PlayerController>().gameObject;
        playerController = player.GetComponent<PlayerController>();


        initialScrollSpeed = scrollSpeed;
        uIManager = FindObjectOfType<UIManager>();
        startTextFrame.transform.position = frameStartPos.position;

        bossDecoy.SetActive(true);

        uIManager.FadeIn();
        StartCoroutine(StartFrameIn());

    }

    private void Update()
    {
        if (isBossBattle)
        {
            if (isReverseScroll)
            {
                StartCoroutine(ReverseScrollStage());
            }
            else if (!isScrollStopped)
            {
                StopScrollStage();
            }
        }
        else
        {
            ScrollStage();
        }

        if (isBossAppeared && boss == null)
        {

            StartCoroutine(StageClear());

        }

    }

    private void ScrollStage()
    {

        walls.transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        backgroundPanel.transform.Translate(Vector3.left * (scrollSpeed / 24) * Time.deltaTime);

    }

    private void StopScrollStage()
    {
        // ステージのスクロールを一時停止
        DOTween.To(() => scrollSpeed, x => scrollSpeed = x, 0f, 2f)
            .OnComplete(() =>
            {
                // 一定時間後に逆向きスクロールを開始
                StartCoroutine(StartReverseScrollDelay());
            });
    }

    public void EndScrollStage()
    {
        // ステージのスクロールを一時停止
        DOTween.To(() => scrollSpeed, x => scrollSpeed = x, 0f, 2f);
    }

    private IEnumerator StartReverseScrollDelay()
    {

        yield return new WaitForSeconds(bossDelayTime);

        // 逆向きスクロールを開始
        isReverseScroll = true;

        playerController.SetPlayerActive(true);

        // 一定時間後にスクロール速度をリセット
        StartCoroutine(ResetScrollSpeedDelay());
    }

    private IEnumerator ResetScrollSpeedDelay()
    {

        yield return new WaitForSeconds(bossReverseDelayTime);

        // スクロール速度をリセット
        scrollSpeed = initialScrollSpeed;

        // スクロールが再開される
        isScrollStopped = false;
    }

    private IEnumerator ReverseScrollStage()
    {
        // 壁を右にスクロール
        walls.transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);
        backgroundPanel.transform.Translate(Vector3.right * (scrollSpeed / 22) * Time.deltaTime);


        yield return new WaitForSecondsRealtime(2f);
        if (!isBossAppeared)
        {
            isBossAppeared = true;
            InstantiateBoss();
        }
    }

    public void StartBossBattle()
    {
        isBossBattle = true;

        // スクロール速度をボス戦時の速度に変更
        scrollSpeed = bossScrollSpeed;

        // ボス戦が始まったことを他のオブジェクトやスクリプトに通知する処理
    }

    IEnumerator StartFrameIn()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        startTextFrame.transform.DOMove(frameStopPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(2f);

        startTextFrame.transform.DOMove(frameEndPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(1f);
    }

    public IEnumerator WarningBeforBossBattle()
    {
        playerController.SetPlayerActive(false);
        player.transform.DOMove(new Vector3(playerStayPos.position.x, playerStayPos.position.y, playerStayPos.position.z), 4f);
        BGMManager.instance.StopBGM();
        StartCoroutine(player.GetComponent<PlayerController>().PlayWarningSE(4f));

        warningPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        warningPanel.SetActive(false);

        yield return new WaitForSecondsRealtime(1f);

        BGMManager.instance.PlayBGM(stage2BossBGM);

    }

    public void InstantiateBoss()
    {
        bossDecoy.SetActive(false);
        boss = Instantiate(bossPrefab, bossStartPos.position, Quaternion.identity);
    }

    IEnumerator StageClear()
    {
        yield return new WaitForSeconds(1f);
        uIManager.FadeOut();
        yield return new WaitForSeconds(2.3f);

        SceneManager.LoadScene("Stage2ED1");
    }
}