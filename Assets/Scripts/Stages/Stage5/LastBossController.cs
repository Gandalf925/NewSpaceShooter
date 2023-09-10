using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class LastBossController : MonoBehaviour
{

    enum BossState
    {
        Normal,
        Angry,
        Mad
    }

    public int maxHP = 1;
    public float currentHP;

    public GameObject player;
    public Transform[] BossPositions;
    [SerializeField] Sprite bossNormal;
    [SerializeField] Sprite bossUseMagic;
    [SerializeField] Sprite bossAngry;
    [SerializeField] Sprite bossTired;

    [SerializeField] GameObject bossShield;
    BossState bossState = BossState.Normal;



    [SerializeField] GameObject bossUseMagicEffect;



    public GameObject[] asteroidPrefabs; // AsteroidのPrefab
    public Transform[] asteroidPoints; // Asteroidが生成される場所

    [SerializeField] GameObject chargeFirePrefab;

    private BoxCollider col;

    private SpriteRenderer spriteRenderer;
    _2dxFX_NewTeleportation2 telepotation;

    GameManager gameManager;
    void Start()
    {
        transform.position = BossPositions[9].position;

        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        telepotation = GetComponent<_2dxFX_NewTeleportation2>();
        col = GetComponent<BoxCollider>();
        telepotation._Fade = 0f;
        player = GameObject.FindWithTag("Player");
        // soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        StartCoroutine(BossMoveRoutine());
    }

    void Update()
    {
        // HP が 0 の場合を先に評価
        if (currentHP == 0)
        {
            bossState = BossState.Mad;
            spriteRenderer.sprite = bossTired;
        }
        else if (currentHP <= maxHP / 2)
        {
            bossState = BossState.Angry;
            spriteRenderer.sprite = bossAngry;
        }

        if (bossState == BossState.Mad)
        {

        }
    }

    IEnumerator BossMoveRoutine()
    {
        while (player != null)
        {
            if (bossState == BossState.Normal)
            {
                int randomIndex = Random.Range(0, 2);

                yield return BossMoveLevel1();

                if (randomIndex == 0)
                {
                    yield return AttackChargeFireArrowLevel1();
                }
                else
                {
                    yield return SpawnAndShootAsteroidLevel1();
                }
                yield return new WaitForSeconds(2f);
            }
            else if (bossState == BossState.Angry)
            {
                int randomIndex = Random.Range(0, 2);

                yield return BossMoveLevel2();
                if (randomIndex == 0)
                {
                    yield return AttackChargeFireArrowLevel2();
                }
                else
                {
                    yield return SpawnAndShootAsteroidLevel2();
                }
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                int randomIndex = Random.Range(0, 2);

                yield return BossMoveLevel3();
                if (randomIndex == 0)
                {
                    StartCoroutine(AttackChargeFireArrowLevel2());
                }
                else
                {
                    StartCoroutine(SpawnAndShootAsteroidLevel2());
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }

    IEnumerator BossMoveLevel1()
    {
        spriteRenderer.sprite = bossNormal;

        int randomCount = Random.Range(1, 3);

        for (int i = 0; i < randomCount; i++)
        {
            yield return SingleBossMove(0.2f);
        }
    }

    IEnumerator BossMoveLevel2()
    {
        spriteRenderer.sprite = bossAngry;

        int randomCount = Random.Range(2, 7);

        for (int i = 0; i < randomCount; i++)
        {
            yield return SingleBossMove(0.1f);
        }
    }
    IEnumerator BossMoveLevel3()
    {
        int randomCount = Random.Range(5, 8);

        for (int i = 0; i < randomCount; i++)
        {
            yield return SingleBossMove(0.1f);
        }
        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator SpawnAndShootAsteroidLevel1()
    {

        List<GameObject> createdObjects = new List<GameObject>();  // 生成したオブジェクトを保存するリスト

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
            if (asteroidRandomIndex == 2)
            {
                asteroid.transform.localScale = new Vector3(6f, 6f, 6f);
            }
            else
            {
                asteroid.transform.localScale = new Vector3(7f, 7f, 7f);
            }

            // Asteroidに力を加えてPlayerに向かって飛ばす
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            Vector3 direction = (player.transform.position - spawnPoint.position).normalized;
            rb.AddForce(direction * 500000f, ForceMode.Impulse);

            yield return new WaitForSeconds(1.5f);
        }

        yield return new WaitForSeconds(2f);


        // ルーチンの最後でリスト内のオブジェクトだけを破壊
        foreach (GameObject obj in createdObjects)
        {
            Destroy(obj);
        }
    }

    IEnumerator SpawnAndShootAsteroidLevel2()
    {
        List<GameObject> createdObjects = new List<GameObject>();  // 生成したオブジェクトを保存するリスト

        spriteRenderer.sprite = bossUseMagic;
        bossUseMagicEffect.SetActive(true);
        yield return new WaitForSeconds(0.7f);

        for (int i = 0; i < 6; i++)
        {
            // AsteroidPointsからランダムな位置を選ぶ
            int spawnRandomIndex = Random.Range(0, asteroidPoints.Length);
            int asteroidRandomIndex = Random.Range(0, asteroidPrefabs.Length);
            Transform spawnPoint = asteroidPoints[spawnRandomIndex];

            // Asteroidを生成
            GameObject asteroid = Instantiate(asteroidPrefabs[asteroidRandomIndex], spawnPoint.position, Quaternion.identity);
            if (asteroidRandomIndex == 2)
            {
                asteroid.transform.localScale = new Vector3(6f, 6f, 6f);
            }
            else
            {
                asteroid.transform.localScale = new Vector3(7f, 7f, 7f);
            }

            // Asteroidに力を加えてPlayerに向かって飛ばす
            Rigidbody rb = asteroid.GetComponent<Rigidbody>();
            Vector3 direction = (player.transform.position - spawnPoint.position).normalized;
            rb.AddForce(direction * 500000f, ForceMode.Impulse);

            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(2f);

        // ルーチンの最後でリスト内のオブジェクトだけを破壊
        foreach (GameObject obj in createdObjects)
        {
            Destroy(obj);
        }
    }

    IEnumerator AttackChargeFireArrowLevel1()
    {
        List<GameObject> createdObjects = new List<GameObject>();  // 生成したオブジェクトを保存するリスト

        spriteRenderer.sprite = bossUseMagic;
        bossUseMagicEffect.SetActive(true);

        for (int i = 0; i < 15; i++) // 25個生成
        {
            float randomX = Random.Range(-25f, 25f);
            float randomY = Random.Range(20f, 35f);
            float randomZ = Random.Range(transform.position.z + 7, transform.position.z + 15);

            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

            GameObject chargeFireArrow = Instantiate(chargeFirePrefab, spawnPosition, Quaternion.identity);

            chargeFireArrow.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            yield return new WaitForSeconds(0.1f); // 生成間隔（必要に応じて調整）
        }

        yield return new WaitForSeconds(2f);

        // ルーチンの最後でリスト内のオブジェクトだけを破壊
        foreach (GameObject obj in createdObjects)
        {
            Destroy(obj);
        }
    }

    IEnumerator AttackChargeFireArrowLevel2()
    {
        List<GameObject> createdObjects = new List<GameObject>();  // 生成したオブジェクトを保存するリスト

        spriteRenderer.sprite = bossUseMagic;
        bossUseMagicEffect.SetActive(true);

        for (int i = 0; i < 30; i++) // 25個生成
        {
            float randomX = Random.Range(-25f, 25f);
            float randomY = Random.Range(20f, 35f);
            float randomZ = Random.Range(transform.position.z + 10, transform.position.z + 20);

            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

            GameObject chargeFireArrow = Instantiate(chargeFirePrefab, spawnPosition, Quaternion.identity);

            chargeFireArrow.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

            yield return new WaitForSeconds(0.07f); // 生成間隔（必要に応じて調整）
        }

        yield return new WaitForSeconds(2f);

        // ルーチンの最後でリスト内のオブジェクトだけを破壊
        foreach (GameObject obj in createdObjects)
        {
            Destroy(obj);
        }
    }


    // シングルのボスの動き（出現→消失→移動→出現）をまとめたコルーチン
    IEnumerator SingleBossMove(float duration)
    {
        yield return Dissolve(duration, false); // ボスを消す
        yield return Move();                // ランダムな位置に移動
        yield return Dissolve(duration, true);  // ボスを登場させる
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
            if (currentHP <= 0)
            {
                currentHP = 0;
                bossState = BossState.Mad;
                spriteRenderer.sprite = bossTired;
                bossShield.SetActive(true);
            }
            gameManager.UpdateScore(damage);
        }
    }
}
