using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MagicianPepe : MonoBehaviour
{
    public int maxHP = 1;
    private float currentHP;
    private PlayerController player;
    public GameObject explosionPrefab;
    public GameObject smallPowerupPrefab;
    public GameObject largePowerupPrefab;
    private bool isShowingDamage = false;
    private Image image;
    public Transform startPos;        // 画面右外のスタート位置
    public Transform stopPos;         // 停止する位置
    public Transform bulletSpawner;   // 弾を発射する位置

    public GameObject bulletPrefab;   // 弾のプレハブ

    public float fireRate = 0.2f;     // 弾の発射間隔（秒）
    public float rotationSpeed = 30f; // 回転速度（度/秒）

    private bool hasReachedStopPos = false;
    private bool isShooting = false;
    private Color originalColor;


    [Header("Manager")]
    GameManager gameManager;
    BGMManager soundManager;

    void Start()
    {
        currentHP = maxHP;
        image = GetComponent<Image>();
        originalColor = image.color;
        player = FindObjectOfType<PlayerController>();
        startPos = GameObject.FindGameObjectWithTag("EliteStartPos").transform;
        stopPos = GameObject.FindGameObjectWithTag("EliteStopPos").transform;
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        // スタート位置から停止位置まで移動
        transform.position = startPos.position;
        transform.DOMove(stopPos.position, 1f).OnComplete(() => hasReachedStopPos = true);
    }

    void Update()
    {
        if (hasReachedStopPos && !isShooting)
        {
            // 停止位置に到達したら、弾を連射
            isShooting = true;
            InvokeRepeating("ShootBullets", fireRate, fireRate);

            // 1秒後に弾の発射位置を回転させる
            Invoke("RotateBulletSpawner", 1f);
        }

        // BulletSpawner の角度を回転させる
        RotateBulletSpawner();
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
                GeneratePowerUpItem();
                player.GetComponent<PlayerController>().PlayExplosionSE();


                Destroy(explosion, 0.5f);

                Destroy(gameObject);
            }

            Destroy(other.gameObject);
        }
    }

    void ShootBullets()
    {
        // 上下左右から弾を発射
        // 上下左右から弾を発射
        ShootBullet(bulletSpawner.TransformDirection(Vector3.up));
        ShootBullet(bulletSpawner.TransformDirection(Vector3.down));
        ShootBullet(bulletSpawner.TransformDirection(Vector3.left));
        ShootBullet(bulletSpawner.TransformDirection(Vector3.right));
    }

    void ShootBullet(Vector2 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawner.position, Quaternion.identity);
        Vector2 lookDirection = direction; // 弾の発射方向
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg; // ベクトルの角度を計算
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle)); // 弾の回転を設定
        bullet.GetComponent<Rigidbody2D>().velocity = lookDirection * 5f; // 弾の速度を設定
    }

    void RotateBulletSpawner()
    {
        // BulletSpawner を回転させる
        bulletSpawner.Rotate(new Vector3(0f, 0f, rotationSpeed * Time.deltaTime));
    }

    private IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float blinkInterval = 0.07f;

        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        image.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        image.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);

        image.color = originalColor;

        isShowingDamage = false;
    }

    private void GeneratePowerUpItem()
    {
        // 小アイテムと大アイテムの確率を設定
        float smallProbability = 0.8f;  // 80％の確率で小アイテムを生成する

        // ランダムな値を生成して、小アイテムか大アイテムを決定する
        float randomValue = Random.value;
        if (randomValue < smallProbability)
        {
            // 小アイテムを生成する
            GameObject smallPowerup = Instantiate(smallPowerupPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // 大アイテムを生成する
            GameObject powerup = Instantiate(largePowerupPrefab, transform.position, Quaternion.identity);
        }
    }
}