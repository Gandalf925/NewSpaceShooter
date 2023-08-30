using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossController : MonoBehaviour
{

    public int maxHP = 1;
    public float currentHP;

    public GameObject player;
    public Transform[] BossPositions;

    private BoxCollider col;

    private bool isShowingDamage = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    _2dxFX_NewTeleportation2 telepotation;

    GameManager gameManager;
    void Start()
    {
        transform.position = BossPositions[9].position;

        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        telepotation = GetComponent<_2dxFX_NewTeleportation2>();
        col = GetComponent<BoxCollider>();
        originalColor = spriteRenderer.color;
        telepotation._Fade = 0f;
        player = GameObject.FindWithTag("Player");
        // soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        StartCoroutine(BossMoveRoutine());

    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator BossMoveRoutine()
    {
        while (player != null)
        {
            yield return BossMoveLevel1();
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator BossMoveLevel1()
    {
        // 1または2回の動作をランダムで決定
        if (currentHP > maxHP / 2)
        {
            int randomCount = Random.Range(1, 3);

            for (int i = 0; i < randomCount; i++)
            {
                yield return SingleBossMove();
            }
        }
        else
        {
            int randomCount = Random.Range(2, 5);
            for (int i = 0; i < randomCount; i++)
            {

                yield return SingleBossMove();
            }
        }


    }

    // シングルのボスの動き（出現→消失→移動→出現）をまとめたコルーチン
    IEnumerator SingleBossMove()
    {
        yield return Dissolve(0.2f, false); // ボスを消す
        yield return Move();                // ランダムな位置に移動
        yield return Dissolve(0.2f, true);  // ボスを登場させる
    }

    IEnumerator Dissolve(float duration, bool appear)
    {
        col.isTrigger = false;
        Vector3 originalPosition = transform.position;  // 位置を保存
        float startFade = appear ? 1f : 0f;
        float endFade = appear ? 0f : 1f;

        float elapsedTime = 0f;



        while (elapsedTime < duration)
        {
            float fade = Mathf.Lerp(startFade, endFade, elapsedTime / duration);
            telepotation._Fade = fade;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ディゾルブ処理の最終値を設定
        telepotation._Fade = endFade;

        transform.position = originalPosition;  // 位置を復元
        col.isTrigger = true;
    }

    IEnumerator Move()
    {
        // BossPositions内からランダムな位置を選ぶ
        int randomIndex = Random.Range(0, BossPositions.Length);
        transform.position = BossPositions[randomIndex].position;

        yield return null;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            int damage = other.GetComponent<Player3DBulletController>().attackPower;
            currentHP -= damage;
            gameManager.UpdateScore(damage);

            // ダメージを受けた際の演出
            StartCoroutine(ShowDamageRoutine());
        }
    }


    private IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float blinkInterval = 0.07f;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = originalColor;

        isShowingDamage = false;
    }

}
