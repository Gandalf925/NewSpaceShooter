using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage2Manager : MonoBehaviour
{
    public float scrollSpeed = 5f;  // ステージのスクロール速度
    public float bossScrollSpeed = 2f;  // ボス戦時のスクロール速度
    public float bossDelayTime = 2f;  // ボス戦開始前の遅延時間
    public float bossReverseDelayTime = 3f;  // ボス戦開始後の逆向きスクロール開始までの遅延時間

    private bool isBossBattle = false;  // ボス戦が開始されたかどうか
    private bool isReverseScroll = false;  // 逆向きスクロールが開始されたかどうか
    private bool isScrollStopped = false;  // スクロールが一時停止中かどうか

    private float initialScrollSpeed;  // 初期のスクロール速度

    public GameObject walls;  // 画面上下の壁オブジェクト

    private List<Transform> wallList;  // 壁オブジェクトのリスト

    void Start()
    {
        initialScrollSpeed = scrollSpeed;

        // 壁オブジェクトの子オブジェクトをリストに追加
        wallList = new List<Transform>();
        foreach (Transform child in walls.transform)
        {
            wallList.Add(child);
        }
    }

    void Update()
    {
        if (isBossBattle)
        {
            if (isReverseScroll)
            {
                ReverseScrollStage();
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
    }

    void ScrollStage()
    {
        // ステージを左にスクロール
        transform.Translate(Vector3.left * scrollSpeed * Time.deltaTime);

        // 背景の移動
        // (背景オブジェクトに対して適切なスクロール処理を行う必要があります)

    }

    void StopScrollStage()
    {
        // ステージのスクロールを一時停止
        scrollSpeed = 0f;

        // 一定時間後に逆向きスクロールを開始
        StartCoroutine(StartReverseScrollDelay());
    }

    IEnumerator StartReverseScrollDelay()
    {
        yield return new WaitForSeconds(bossDelayTime);

        // 逆向きスクロールを開始
        isReverseScroll = true;

        // 一定時間後にスクロール速度をリセット
        StartCoroutine(ResetScrollSpeedDelay());
    }

    IEnumerator ResetScrollSpeedDelay()
    {
        yield return new WaitForSeconds(bossReverseDelayTime);

        // スクロール速度をリセット
        scrollSpeed = initialScrollSpeed;

        // スクロールが再開される
        isScrollStopped = false;
    }

    void ReverseScrollStage()
    {
        // ステージを右にスクロール
        transform.Translate(Vector3.right * scrollSpeed * Time.deltaTime);

        // 背景の移動
        // (背景オブジェクトに対して逆向きのスクロール処理を行う必要があります)
    }

    public void StartBossBattle()
    {
        isBossBattle = true;

        // スクロール速度をボス戦時の速度に変更
        scrollSpeed = bossScrollSpeed;

        // ボス戦が始まったことを他のオブジェクトやスクリプトに通知する処理
    }
}