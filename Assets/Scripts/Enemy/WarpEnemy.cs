using System.Collections;
using UnityEngine;
using DG.Tweening;


public class WarpEnemy : MonoBehaviour
{

    public int maxHP = 30;
    public float currentHP;
    public float speed = 5f;
    public GameObject player;
    private Rigidbody2D rb;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    public GameObject explosionPrefab;

    public GameObject enemyBulletPrefab;  // 敵の弾のプレファブ
    public float fireRate = 1f;  // 1秒に1回弾を発射する

    private float currentAngle = 325f;  // Current angle to fire the bullet
    private float angleStep = 8.3f;  // Angle step to increase/decrease the angle
    private bool isIncreasing = false;

    public GameObject smallPowerupPrefab;
    public GameObject largePowerupPrefab;

    private bool isShowingDamage = false;
    private Color originalColor;
    private Material dissolveMaterial;

    GameManager gameManager;

    _2dxFX_NewTeleportation2 telepotation;

    void Start()
    {
        telepotation = GetComponent<_2dxFX_NewTeleportation2>();
        currentHP = maxHP;
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        dissolveMaterial = GetComponent<SpriteRenderer>().material;
        originalColor = spriteRenderer.color;

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        StartCoroutine(EnemyRoutine());
    }

    void Update()
    {
        // Change the direction if the angle reaches 325 or 225
        if (currentAngle >= 325f)
            isIncreasing = false;
        else if (currentAngle <= 225f)
            isIncreasing = true;

        // Update the current angle
        currentAngle += (isIncreasing ? angleStep : -angleStep);
    }

    IEnumerator EnemyRoutine()
    {
        while (currentHP != 0)
        {
            yield return MoveRoutine();

            yield return new WaitForSeconds(0.5f);

            // ディゾルブ処理をかけながら出現
            yield return DissolveRoutine(0.5f, true);

            // 0.7秒待機
            yield return new WaitForSeconds(1f);

            // 弾を発射
            yield return FireFanShapedBullets();

            yield return new WaitForSeconds(0.3f);

            // ディゾルブ処理をかけながら消失
            yield return DissolveRoutine(0.5f, false);
        }
    }


    private IEnumerator FireFanShapedBullets()
    {
        for (int i = 0; i <= 20; i++)
        {
            yield return new WaitForSeconds(0.01f);

            float bulletDirX = transform.position.x + Mathf.Sin(currentAngle * Mathf.Deg2Rad);  // X-coordinate calculation
            float bulletDirY = transform.position.y + Mathf.Cos(currentAngle * Mathf.Deg2Rad);  // Y-coordinate calculation

            Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY);  // The vector of the bullet
            Vector3 bulletMoveDirection = (bulletVector - transform.position).normalized;  // The direction of the bullet movement

            GameObject tmp = Instantiate(enemyBulletPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(bulletMoveDirection.y, bulletMoveDirection.x) * Mathf.Rad2Deg)));
            tmp.GetComponent<Rigidbody2D>().velocity = new Vector2(bulletMoveDirection.x, bulletMoveDirection.y) * speed;

            // Change the direction if the angle reaches 325 or 225
            if (currentAngle >= 325f)
                isIncreasing = false;
            else if (currentAngle <= 225f)
                isIncreasing = true;

            // Update the current angle
            currentAngle += (isIncreasing ? angleStep : -angleStep);
        }
    }

    IEnumerator MoveRoutine()
    {
        // ランダムな位置に移動
        float x = Random.Range(2f, 8f);
        float y = Random.Range(0f, 4f);
        transform.position = new Vector2(x, y);

        yield return null;
    }

    IEnumerator DissolveRoutine(float duration, bool appear)
    {
        float startFade = appear ? 1f : 0f;
        float endFade = appear ? 0f : 1f;

        float elapsedTime = 0f;

        col.isTrigger = false;

        while (elapsedTime < duration)
        {
            float fade = Mathf.Lerp(startFade, endFade, elapsedTime / duration);
            telepotation._Fade = fade;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ディゾルブ処理の最終値を設定
        telepotation._Fade = endFade;

        col.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            int damage = collision.GetComponent<PlayerBulletController>().attackPower;
            currentHP -= damage;
            gameManager.UpdateScore(damage);

            StartCoroutine(ShowDamageRoutine());

            if (currentHP <= 0)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.DOScale(new Vector3(50f, 50f, 0), 0.5f);
                explosion.GetComponent<SpriteRenderer>().DOColor(new Color(255, 0, 0, 0), 0.5f);
                GeneratePowerUpItem();
                player.GetComponent<PlayerController>().PlayExplosionSE();

                Destroy(explosion, 0.5f);
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
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

    private void GeneratePowerUpItem()
    {
        float smallProbability = 0.8f;  // 80％の確率で小アイテムを生成する

        float randomValue = Random.value;
        if (randomValue < smallProbability)
        {
            GameObject smallPowerup = Instantiate(smallPowerupPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject powerup = Instantiate(largePowerupPrefab, transform.position, Quaternion.identity);
        }
    }
}