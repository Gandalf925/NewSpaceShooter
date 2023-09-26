using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;

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
    public Player3DController playerController;
    public Transform[] BossPositions;
    [SerializeField] Transform bossDeadPos;
    [SerializeField] Sprite bossNormal;
    [SerializeField] Sprite bossUseMagic;
    [SerializeField] Sprite bossAngry;
    [SerializeField] Sprite bossTired;
    [SerializeField] Sprite bossDead;

    [SerializeField] GameObject bossShield;
    [SerializeField] GameObject explosionEffect;

    BossState bossState = BossState.Normal;
    Coroutine bossMoveCoroutine;
    Coroutine chargeFireAttackCoroutine;
    Coroutine AsteroidAttackCoroutine;
    public Transform[] bossExplosionPoints;

    [SerializeField] AudioClip bossExplosionSE;



    [SerializeField] GameObject bossUseMagicEffect;



    public GameObject[] asteroidPrefabs; // AsteroidのPrefab
    public Transform[] asteroidPoints; // Asteroidが生成される場所

    [SerializeField] GameObject chargeFirePrefab;

    private BoxCollider col;

    private SpriteRenderer spriteRenderer;
    _2dxFX_NewTeleportation2 telepotation;

    bool isBossMad = false;

    GameManager gameManager;
    Stage5Manager stage5Manager;

    public AudioSource seSource;
    public AudioClip warpSE;

    void Start()
    {
        transform.position = new Vector3(BossPositions[5].position.x, bossDeadPos.position.y, BossPositions[5].position.z);

        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        telepotation = GetComponent<_2dxFX_NewTeleportation2>();
        col = GetComponent<BoxCollider>();
        telepotation._Fade = 0f;
        player = GameObject.FindWithTag("Player");
        playerController = player.GetComponent<Player3DController>();
        seSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        stage5Manager = FindObjectOfType<Stage5Manager>();

        playerController.DisableShooting();
        bossMoveCoroutine = StartCoroutine(BossMoveRoutine());
    }

    void Update()
    {
        if (bossState != BossState.Mad)
        {
            if (currentHP <= maxHP / 2)
            {
                bossState = BossState.Angry;
                spriteRenderer.sprite = bossAngry;
            }

            if (currentHP <= 0)
            {
                if (bossState != BossState.Mad)
                {
                    currentHP = 0; // 保険として currentHP を 0 にクリッピング
                    bossState = BossState.Mad;
                    spriteRenderer.sprite = bossTired;
                }
            }
        }

        if (bossState == BossState.Mad && isBossMad == false)
        {
            isBossMad = true;
            StartCoroutine(stage5Manager.StartLastEvent());
        }
    }

    IEnumerator BossMoveRoutine()
    {
        transform.DOMoveY(BossPositions[5].position.y, 5f);
        Transform cameraTransform = Camera.main.transform;
        Tweener bossMoveTween = transform.DOMoveY(BossPositions[5].position.y, 5f);
        Tweener cameraShakeTween = cameraTransform.DOShakePosition(5f, 0.5f, 100, 90, false, true);


        yield return new WaitForSeconds(8f);

        playerController.EnableShooting();

        while (player != null)
        {
            if (bossState == BossState.Normal)
            {
                int randomIndex = Random.Range(0, 2);

                yield return BossMoveLevel1();

                if (randomIndex == 0)
                {
                    spriteRenderer.sprite = bossUseMagic;
                    yield return AttackChargeFireArrowLevel1();
                }
                else
                {
                    spriteRenderer.sprite = bossUseMagic;
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
                    spriteRenderer.sprite = bossUseMagic;
                    yield return AttackChargeFireArrowLevel2();
                }
                else
                {
                    spriteRenderer.sprite = bossUseMagic;
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
                    chargeFireAttackCoroutine = StartCoroutine(AttackChargeFireArrowLevel2());
                }
                else
                {
                    AsteroidAttackCoroutine = StartCoroutine(SpawnAndShootAsteroidLevel2());
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
        seSource.PlayOneShot(warpSE);
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
                bossState = BossState.Mad;
                spriteRenderer.sprite = bossTired;
                bossShield.SetActive(true);
            }
            gameManager.UpdateScore(damage);
        }

        if (other.CompareTag("SpecialBullet"))
        {
            StartCoroutine(BossDead());
        }
    }

    IEnumerator BossDead()
    {
        stage5Manager.HideReleaseText();
        stage5Manager.player.DisableShooting();
        yield return PauseGameForHalfSeconds();

        AllAttackDelete();
        if (bossMoveCoroutine != null)
        {
            StopCoroutine(bossMoveCoroutine);
        }

        ChangeBossSpriteDead();
        seSource.PlayOneShot(bossExplosionSE);

        StartCoroutine(GenerateBossExplosionEffects());

        transform.DOMoveY(bossDeadPos.position.y, 5f);

        yield return new WaitForSeconds(5f);
        stage5Manager.uIManager.FadeOut();
        yield return new WaitForSeconds(2f);

        BGMManager.instance.StopBGM();

        SceneManager.LoadScene("ED1");
    }

    IEnumerator PauseGameForHalfSeconds()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1;
    }
    void ChangeBossSpriteDead()
    {
        telepotation._Fade = 0f;
        spriteRenderer.sprite = bossDead;
    }
    void AllAttackDelete()
    {
        bossUseMagicEffect.SetActive(false);

        if (chargeFireAttackCoroutine != null)
        {
            StopCoroutine(chargeFireAttackCoroutine);
        }
        if (AsteroidAttackCoroutine != null)
        {
            StopCoroutine(AsteroidAttackCoroutine);
        }

        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroids");
        GameObject[] chargeFires = GameObject.FindGameObjectsWithTag("ChargeFire");
        GameObject[] enemyBullets = GameObject.FindGameObjectsWithTag("EnemyBullet");

        foreach (GameObject asteroid in asteroids)
        {
            Destroy(asteroid);
        }
        foreach (GameObject chargeFire in chargeFires)
        {
            Destroy(chargeFire);
        }
        foreach (GameObject enemyBullet in enemyBullets)
        {
            Destroy(enemyBullet);
        }
    }

    IEnumerator GenerateBossExplosionEffects()
    {
        float duration = 5.0f; // 総持続時間
        float interval = 0.5f; // エフェクト生成の間隔

        float timer = 0.0f; // タイマー初期化

        while (timer < duration)
        {
            // bossExplosionPoints配列の中からランダムなTransformを選ぶ
            int randomIndex = Random.Range(0, bossExplosionPoints.Length);
            Transform randomPoint = bossExplosionPoints[randomIndex];

            // ランダムなTransformの位置にエクスプロージョンエフェクトを生成
            Instantiate(explosionEffect, randomPoint.position, Quaternion.identity);

            timer += interval; // タイマーを更新

            yield return new WaitForSeconds(interval); // 指定した間隔で待機
        }
    }
}
