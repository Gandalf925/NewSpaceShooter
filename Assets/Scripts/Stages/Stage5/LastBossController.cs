using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossController : MonoBehaviour
{

    public int maxHP = 1;
    public float currentHP;

    public GameObject player;
    public Transform[] BossPositions;
    [SerializeField] Sprite bossNormal;
    [SerializeField] Sprite bossSmile;
    [SerializeField] Sprite bossUseMagic;
    [SerializeField] Sprite bossAngry;
    [SerializeField] Sprite bossSad;

    [SerializeField] GameObject bossUseMagicEffect;



    public GameObject[] asteroidPrefabs; // AsteroidのPrefab
    public Transform[] asteroidPoints; // Asteroidが生成される場所

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
            yield return SpawnAndShootAsteroidLevel2();
            yield return new WaitForSeconds(3f);
        }
    }

    IEnumerator BossMoveLevel1()
    {
        spriteRenderer.sprite = bossNormal;

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


    IEnumerator SpawnAndShootAsteroidLevel1()
    {
        spriteRenderer.sprite = bossUseMagic;
        bossUseMagicEffect.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        // AsteroidPointsからランダムな位置を選ぶ
        int spawnRandomIndex = Random.Range(0, asteroidPoints.Length);
        int asteroidRandomIndex = Random.Range(0, asteroidPrefabs.Length);
        Transform spawnPoint = asteroidPoints[spawnRandomIndex];

        // Asteroidを生成
        GameObject asteroid = Instantiate(asteroidPrefabs[asteroidRandomIndex], spawnPoint.position, Quaternion.identity);
        asteroid.transform.localScale = new Vector3(7f, 7f, 7f);

        // Asteroidに力を加えてPlayerに向かって飛ばす
        Rigidbody rb = asteroid.GetComponent<Rigidbody>();
        Vector3 direction = (player.transform.position - spawnPoint.position).normalized;
        rb.AddForce(direction * 500000f, ForceMode.Impulse);

        yield return new WaitForSeconds(2f);

        bossUseMagicEffect.SetActive(false);
        spriteRenderer.sprite = bossNormal;

        yield return new WaitForSeconds(3f);

        Destroy(asteroid);
    }

    IEnumerator SpawnAndShootAsteroidLevel2()
    {
        spriteRenderer.sprite = bossUseMagic;
        bossUseMagicEffect.SetActive(true);
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < 3; i++)
        {
            // AsteroidPointsからランダムな位置を選ぶ
            int spawnRandomIndex = Random.Range(0, asteroidPoints.Length);
            int asteroidRandomIndex = Random.Range(0, asteroidPrefabs.Length);
            Transform spawnPoint = asteroidPoints[spawnRandomIndex];

            // Asteroidを生成
            GameObject asteroid = Instantiate(asteroidPrefabs[asteroidRandomIndex], spawnPoint.position, Quaternion.identity);
            asteroid.transform.localScale = new Vector3(7f, 7f, 7f);

            // Asteroidに力を加えてPlayerに向かって飛ばす
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            Vector3 direction = (player.transform.position - spawnPoint.position).normalized;
            rb.AddForce(direction * 500000f, ForceMode.Impulse);

            yield return new WaitForSeconds(1.5f);
        }

        bossUseMagicEffect.SetActive(false);
        spriteRenderer.sprite = bossNormal;
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
