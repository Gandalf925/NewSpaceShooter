using System.Collections;
using UnityEngine;

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

    private bool isShowingDamage = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private int currentHP;
    private GameManager gameManager;

    void Start()
    {
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // 弾の発射
        StartCoroutine(FireRoutine());
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