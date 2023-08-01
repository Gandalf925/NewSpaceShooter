using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NormalMovePepe : MonoBehaviour
{
    public int maxHP = 1;
    private float currentHP;
    public float moveSpeed = 2f;
    public float bulletSpeed = 10f;
    public float bulletFireRate = 2f;
    public GameObject bulletPrefab;
    public GameObject player;
    public GameObject explosionPrefab;
    public Transform bulletSpawnPoint;
    public GameObject smallPowerupPrefab;
    public GameObject largePowerupPrefab;

    private bool isShowingDamage = false;
    private Image image;
    private Color originalColor;
    GameManager gameManager;

    [Header("Sound")]
    BGMManager soundManager;

    void Start()
    {
        currentHP = maxHP;
        image = GetComponent<Image>();
        originalColor = image.color;
        player = GameObject.FindWithTag("Player");
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // 弾の発射
        StartCoroutine(FireRoutine());
    }

    void Update()
    {
        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);

        if (transform.position.x <= -12f)
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

    private IEnumerator FireRoutine()
    {
        while (true)
        {
            // 0より小さい場合は弾の発射をやめる
            if (transform.position.x < 0) yield break;

            // 発射時に現在のPlayerの位置を取得する
            if (player != null)
            {
                Vector3 targetPosition = player.transform.position;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.transform.right = (targetPosition - bulletSpawnPoint.position).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

                // 現在のPlayerの位置に向かって発射するようにする
                bullet.GetComponent<Rigidbody2D>().velocity = (targetPosition - bulletSpawnPoint.position).normalized * bulletSpeed;
                yield return new WaitForSeconds(bulletFireRate);
            }
            else
            {
                yield return null;
            }
        }
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