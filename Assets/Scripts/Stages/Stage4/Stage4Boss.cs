using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stage4Boss : MonoBehaviour
{
    public int maxHP = 1;
    private float currentHP;
    private PlayerController player;
    public GameObject explosionPrefab;
    public GameObject laserPrefab;
    public Transform laserSpawner;
    private bool isShowingDamage = false;

    private Image image;
    public Transform bossStartPos;    // 画面右外のスタート位置
    public Transform bossStopPosMiddle;     // 停止する位置
    public Transform bossStopPosUp;     // 停止する位置
    public Transform bossStopPosDown;     // 停止する位置
    public bool isDead = false;

    private Color originalColor;


    [Header("Manager")]
    GameManager gameManager;
    Stage4Manager stage4Manager;

    void Start()
    {
        currentHP = maxHP;
        image = GetComponent<Image>();
        originalColor = image.color;
        player = FindObjectOfType<PlayerController>();
        bossStartPos = GameObject.FindGameObjectWithTag("EliteStartPos").transform;
        bossStopPosUp = GameObject.FindGameObjectWithTag("BossStopPosUp").transform;
        bossStopPosMiddle = GameObject.FindGameObjectWithTag("BossStopPosMiddle").transform;
        bossStopPosDown = GameObject.FindGameObjectWithTag("BossStopPosDown").transform;
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        stage4Manager = FindObjectOfType<Stage4Manager>();

        // スタート位置から停止位置まで移動
        transform.position = bossStartPos.position;
        transform.DOMove(bossStopPosMiddle.position, 1f).OnComplete(() => StartCoroutine(stage4Manager.SetShieldPepes()));
        StartCoroutine(StartMoving());
    }

    IEnumerator StartMoving()
    {
        yield return new WaitForSeconds(3f);

        while (player != null)
        {
            // ランダムな移動時間を設定（4秒から7秒の間）
            float randomMoveTime = Random.Range(4f, 6f);

            // 画面内に収まる範囲で上下移動を繰り返す
            StartCoroutine(MoveUpDownCoroutine(randomMoveTime));

            yield return new WaitForSeconds(randomMoveTime); // 移動時間

            StartCoroutine(ShootLaser());

            yield return new WaitForSeconds(8f); // ビーム発射と待機時間
        }
    }

    IEnumerator ShootLaser()
    {
        transform.DOShakePosition(0.5f, 10, 10);
        yield return new WaitForSeconds(1f);
        Instantiate(laserPrefab, laserSpawner.position, Quaternion.identity);
        yield return new WaitForSeconds(5f);
    }

    IEnumerator MoveUpDownCoroutine(float moveTime)
    {
        float maxY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.8f, 0)).y;
        float minY = Camera.main.ViewportToWorldPoint(new Vector3(0, 0.2f, 0)).y;

        float elapsedTime = 0f;
        Transform currentStopPosition = transform; // 初期位置を設定

        while (elapsedTime < moveTime)
        {
            // ランダムな停止位置を選択
            Transform randomStopPos = GetRandomStopPosition(currentStopPosition);

            // ランダムな停止位置に移動
            transform.DOMove(randomStopPos.position, 0.5f).SetEase(Ease.Linear);

            // 現在の停止位置を更新
            currentStopPosition = randomStopPos;

            // 経過時間を加算
            elapsedTime += 0.5f;

            // 待機
            yield return new WaitForSeconds(0.5f);
        }
    }

    Transform GetRandomStopPosition(Transform currentPosition)
    {
        Transform[] possiblePositions = { bossStopPosUp, bossStopPosMiddle, bossStopPosDown };

        // 除外する座標を削除した配列を作成
        List<Transform> availablePositions = new List<Transform>(possiblePositions);
        availablePositions.Remove(currentPosition);

        int randomIndex = Random.Range(0, availablePositions.Count);

        return availablePositions[randomIndex];
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            int damage = other.GetComponent<PlayerBulletController>().attackPower;
            currentHP -= damage;
            gameManager.UpdateScore(damage);

            // ダメージを受けた際の演出
            StartCoroutine(ShowDamageRoutine());

            if (currentHP <= 0)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.DOScale(new Vector3(50f, 50f, 0), 0.5f);
                explosion.GetComponent<SpriteRenderer>().DOColor(new Color(255, 0, 0, 0), 0.5f);
                player.GetComponent<PlayerController>().PlayExplosionSE();
                BossDead();


                Destroy(explosion, 0.5f);

                Destroy(gameObject);
            }

            Destroy(other.gameObject);
        }
    }

    IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float blinkInterval = 0.07f;

        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);

        image.color = originalColor;

        isShowingDamage = false;
    }

    public void BossDead()
    {
        isDead = true;
    }
}
