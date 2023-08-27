using System.Collections;
using UnityEngine;
using DG.Tweening;

public class RadialEnemy : MonoBehaviour
{
    public int maxHP = 5;
    public float bulletSpeed = 10f;
    public float bulletFireRate = 1f;
    public float attackDuration = 5f;

    public GameObject bulletPrefab;
    private BoxCollider2D col;
    public GameObject explosionPrefab;
    public Transform bulletSpawnPoint;
    public PlayerController player;

    public GameObject smallPowerupPrefab;
    public GameObject largePowerupPrefab;

    public GameObject lifePowerupPrefab;

    public float moveSpeed = 1f;
    public float minY = -2.5f;
    public float maxY = 2.5f;

    private bool isShowingDamage = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private float currentHP;
    private GameManager gameManager;

    private int moveDirection;

    [Header("Audio")]
    BGMManager soundManager;


    void Start()
    {

        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        player = FindObjectOfType<PlayerController>();


        // 弾の発射
        StartCoroutine(FireRoutine());


        // 初期移動方向をランダムに決定
        moveDirection = Random.Range(0, 2) * 2 - 1;
    }

    void Update()
    {
        // 上下にゆっくり移動する
        transform.Translate(Vector2.up * moveSpeed * moveDirection * Time.deltaTime);

        // 移動範囲を超えたら反転して移動方向を変える
        if (transform.position.y < minY)
        {
            moveDirection = 1;
        }
        else if (transform.position.y > maxY)
        {
            moveDirection = -1;
        }
    }

    private IEnumerator FireRoutine()
    {
        transform.DOMoveX(transform.position.x - 3f, 3f);

        while (true)
        {
            for (int i = 0; i < 7; i++)
            {
                float angle = Mathf.Lerp(135f, 225f, (float)i / 6f);
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0f, 0f, angle));
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
            }
            yield return new WaitForSeconds(bulletFireRate);
        }
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

                GeneratePowerUpItemAndLife();

                player.PlayExplosionSE();

                Destroy(explosion, 0.5f);
                Destroy(gameObject);
            }

            Destroy(other.gameObject);
        }
    }

    private IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float duration = 0.3f;
        float halfDuration = duration / 4f;

        spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f); // 濃い赤
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.white; // 濃い白
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = new Color(1f, 0.2f, 0.2f, 1f); // 薄い赤
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.white; // 濃い白
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = originalColor; // 通常

        isShowingDamage = false;
    }


    private void GeneratePowerUpItemAndLife()
    {
        // 小アイテムと大アイテムの確率を設定
        float smallProbability = 0.7f;  // 70％の確率で小アイテムを生成する
        float largeProbability = 0.9f;  // 70％の確率で大アイテムを生成する
        // ランダムな値を生成して、小アイテムか大アイテムを決定する
        float randomValue = Random.value;
        if (randomValue < smallProbability)
        {
            // 小アイテムを生成する
            GameObject smallPowerup = Instantiate(smallPowerupPrefab, transform.position, Quaternion.identity);
        }
        else if (smallProbability < randomValue && randomValue < largeProbability)
        {
            // 大アイテムを生成する
            GameObject powerup = Instantiate(largePowerupPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject powerup = Instantiate(lifePowerupPrefab, transform.position, Quaternion.identity);
        }
    }

    private void EnableCollider()
    {
        col.enabled = true;
    }
    private void DisableCollider()
    {
        col.enabled = false;
    }
}