using UnityEngine;
using DG.Tweening;

public class RollingEnemy : MonoBehaviour
{
    public float approachSpeed = 5f; // 近づく速度
    public float maxHP = 1; // Maximum health points
    private float currentHP; // Current health points
    private bool isMoving = false; // 移動中かどうかのフラグ
    public GameObject explosionPrefab;
    public GameObject smallPowerupPrefab;
    public GameObject largePowerupPrefab;

    private Transform player; // プレイヤーの位置

    [Header("Sound")]
    BGMManager soundManager;

    private void Start()
    {
        // プレイヤーの位置を見つける
        player = FindObjectOfType<PlayerController>().transform;
        // Initialize currentHP to maxHP
        currentHP = maxHP;
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
    }

    private void Update()
    {
        // Move towards the player
        if (isMoving)
        {
            if (player != null)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.Translate(direction * approachSpeed * Time.deltaTime);
            }

            // プレイヤーのx軸から-1以下の位置に来たら移動を停止する
            if (transform.position.x <= player.position.x - 1f)
            {
                isMoving = false;
            }
        }
    }

    public void StartMoving()
    {
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("PlayerBullet"))
        {
            // 弾のダメージを取得してHPから減らす
            PlayerBulletController bullet = collision.gameObject.GetComponent<PlayerBulletController>();
            currentHP -= bullet.attackPower;

            // HPが0以下になったら敵を破壊する
            if (currentHP <= 0)
            {
                GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                explosion.transform.DOScale(new Vector3(50f, 50f, 0), 0.5f);
                explosion.GetComponent<SpriteRenderer>().DOColor(new Color(255, 0, 0, 0), 0.5f);
                GeneratePowerUpItem();
                player.GetComponent<PlayerController>().PlayExplosionSE();


                Destroy(explosion, 3f);

                Destroy(gameObject);
            }

            // 弾を破壊する
            Destroy(collision.gameObject);
        }
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