using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Stage2Boss : MonoBehaviour
{
    private int maxHP = 700;
    public int currentHP;

    public GameObject explosionPrefab;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    GameManager gameManager;


    public bool isDefeated = false;


    public Transform bulletSpawner;  // Bulletを発射する位置
    public GameObject reflectiveBulletPrefab;  // 反射するBulletのプレハブ

    PlayerController player;

    private float rotationSpeed = 200f;  // Bossの回転速度
    private float bulletFireInterval = 3f;  // Bulletの発射間隔
    private float bulletSpeed = 10f;  // Bulletの移動速度
    private float verticalSpeed = 2f;  // 上下移動の速度
    private float minY = -2.5f;  // 移動の下限Y座標
    private float maxY = 2.5f;  // 移動の上限Y座標

    private float nextBulletFireTime;  // 次にBulletを発射する時刻

    private bool movingUp = true;  // 上方向に移動しているかどうかのフラグ




    private void Start()
    {
        DOTween.SetTweensCapacity(1000, 100);
        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        player = FindObjectOfType<PlayerController>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // 最初のBullet発射時刻を設定
        nextBulletFireTime = Time.time + bulletFireInterval;


    }

    private void Update()
    {
        // Bossの回転
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 上下移動
        MoveVertically();

        // HPに応じて色を変化させる
        UpdateBossColor();


        // Bulletの発射
        if (Time.time >= nextBulletFireTime)
        {
            FireReflectiveBullet();
            nextBulletFireTime = Time.time + bulletFireInterval;
        }
    }

    private void MoveVertically()
    {
        float verticalMovement = transform.position.y;

        // 上方向に移動中の場合
        if (movingUp)
        {
            verticalMovement += verticalSpeed * Time.deltaTime;

            transform.Translate(Vector3.right * 1f * Time.deltaTime);

            // 上限Y座標に到達したら反転
            if (verticalMovement >= maxY)
            {
                verticalMovement = maxY;
                movingUp = false;
            }
        }
        // 下方向に移動中の場合
        else
        {
            verticalMovement -= verticalSpeed * Time.deltaTime;

            transform.Translate(Vector3.left * 1f * Time.deltaTime);

            // 下限Y座標に到達したら反転
            if (verticalMovement <= minY)
            {
                verticalMovement = minY;
                movingUp = true;
            }
        }

        transform.position = new Vector3(transform.position.x, verticalMovement, transform.position.z);
    }

    private void FireReflectiveBullet()
    {

        if (player.isActive)
        {
            if (currentHP > 0)
            {
                // ReflectiveBulletを生成し、BulletSpawnerから発射
                GameObject bulletInstance = Instantiate(reflectiveBulletPrefab, bulletSpawner.position, Quaternion.identity);
                ReflectiveBullet reflectiveBullet = bulletInstance.GetComponent<ReflectiveBullet>();

                // 反射するBulletの移動速度を設定
                reflectiveBullet.speed = bulletSpeed;

                // プレイヤーの座標上下±45度に向けた発射角度を設定
                Vector3 playerPosition = player.transform.position;
                Vector3 targetDirection = (playerPosition - bulletSpawner.position).normalized;
                Vector3 bulletDirection = Quaternion.Euler(0f, 0f, Random.Range(-45f, 45f)) * targetDirection;

                Rigidbody2D bulletRb = bulletInstance.GetComponent<Rigidbody2D>();
                bulletRb.velocity = bulletDirection * bulletSpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            int damage = collision.GetComponent<PlayerBulletController>().attackPower;
            currentHP -= damage;
            gameManager.UpdateScore(damage);


            if (currentHP <= 0)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.DOScale(new Vector3(50f, 50f, 0), 0.5f);
                explosion.GetComponent<SpriteRenderer>().DOColor(new Color(255, 0, 0, 0), 0.5f);

                Destroy(explosion, 0.5f);
                Destroy(gameObject);

                isDefeated = true;
            }

            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            // 上下移動の方向を反転させる
            movingUp = !movingUp;
        }
    }

    private void UpdateBossColor()
    {
        float healthPercentage = (float)currentHP / maxHP;
        Color targetColor = new Color(1f, 1f - healthPercentage, 1f - healthPercentage, 1f);
        spriteRenderer.color = Color.Lerp(originalColor, targetColor, 1f - healthPercentage);
    }

    private bool IsBossInsideScreen()
    {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;
    }
}