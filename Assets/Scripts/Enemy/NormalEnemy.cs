using System.Collections;
using UnityEngine;
using DG.Tweening;

public class NormalEnemy : MonoBehaviour
{
    public int maxHP = 1;
    public float moveSpeed = 2f;
    public float bulletSpeed = 10f;
    public float bulletFireRate = 2f;
    public GameObject bulletPrefab;
    public GameObject player;
    public GameObject explosionPrefab;
    public Transform bulletSpawnPoint;


    private int currentHP;
    private bool isShowingDamage = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    GameManager gameManager;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        player = GameObject.FindWithTag("Player");

        // 弾の発射
        StartCoroutine(FireRoutine());
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < -0.1f)
        {
            Destroy(gameObject);
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
                Destroy(explosion, 1f);
                Destroy(gameObject);
            }

            Destroy(other.gameObject);
        }
    }

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            // 発射時に現在のPlayerの位置を取得する
            Vector3 targetPosition = player.transform.position;

            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            bullet.transform.right = (targetPosition - bulletSpawnPoint.position).normalized;
            bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

            // 現在のPlayerの位置に向かって発射するようにする
            bullet.GetComponent<Rigidbody2D>().velocity = (targetPosition - bulletSpawnPoint.position).normalized * bulletSpeed;
            yield return new WaitForSeconds(1f / bulletFireRate);
        }
    }

    private IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float duration = 0.4f;
        float halfDuration = duration / 2f;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.Lerp(Color.white, originalColor, 0.5f);
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = originalColor;

        isShowingDamage = false;
    }
}