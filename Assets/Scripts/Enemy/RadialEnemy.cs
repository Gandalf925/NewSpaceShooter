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
    public GameObject explosionPrefab;
    public Transform bulletSpawnPoint;
    public GameObject player;

    public float moveSpeed = 1f;
    public float minY = -2.5f;
    public float maxY = 2.5f;

    private bool isShowingDamage = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private float currentHP;
    private GameManager gameManager;

    private int moveDirection;


    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

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
        while (true)
        {
            for (int i = 0; i < 7; i++)
            {
                float angle = Mathf.Lerp(135f, 225f, (float)i / 6f);
                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.Euler(0f, 0f, angle));
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;
            }
            yield return new WaitForSeconds(1f / bulletFireRate);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            float damage = other.GetComponent<PlayerBulletController>().attackPower;
            currentHP -= damage;
            gameManager.UpdateScore(damage);

            // ダメージを受けた際の演出
            StartCoroutine(ShowDamageRoutine());

            if (currentHP <= 0)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.DOScale(new Vector3(50f, 50f, 0), 0.5f);
                Destroy(explosion, 1f);
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
}