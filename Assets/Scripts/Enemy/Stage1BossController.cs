using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Stage1BossController : MonoBehaviour
{

    // Variables related to boss properties
    public int maxHP = 100;
    public float moveSpeed = 2f;
    public GameObject player;
    public GameObject explosionPrefab;
    public bool isDefeated = false;

    // Variables related to beam properties
    public float beamSpeed = 5f;
    public float beamFireRate = 8f;
    public GameObject beamPrefab;
    public Transform beamSpawner;

    // Variables related to bullet properties
    public float bulletSpeed = 7f;
    public float bulletFireRate = 8f;
    public int bulletCount = 16;
    public GameObject bulletPrefab;
    public Transform[] bulletSpawners;

    // Variables related to boss state
    private float currentHP;
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
        StartCoroutine(FireBeamRoutine());
        StartCoroutine(FireBarrageRoutine());
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (currentHP > 0)
        {
            // 左右に移動
            transform.position += new Vector3(Mathf.Cos(Time.time) * moveSpeed * Time.deltaTime, 0f, 0f);

            // 上下に移動
            transform.position += new Vector3(0f, Mathf.Sin(Time.time) * moveSpeed * Time.deltaTime, 0f);
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

                Destroy(explosion, 0.5f);
                Destroy(gameObject);

                isDefeated = true;
            }

            Destroy(other.gameObject);
        }
    }

    private IEnumerator FireBeamRoutine()
    {
        while (player != null)
        {
            // 発射時に現在のPlayerの位置を取得する
            if (currentHP > 0)
            {
                Vector3 targetPosition = player.transform.position;

                GameObject bullet = Instantiate(beamPrefab, beamSpawner.position, beamSpawner.rotation);
                bullet.transform.right = (targetPosition - beamSpawner.position).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * beamSpeed;

                // 現在のPlayerの位置に向かって発射するようにする
                bullet.GetComponent<Rigidbody2D>().velocity = (targetPosition - beamSpawner.position).normalized * beamSpeed;
                yield return new WaitForSeconds(beamFireRate);
            }
            else
            {
                yield return null;
            }
        }
    }

    IEnumerator FireBarrageRoutine()
    {
        // 弾幕の発射角度をランダムに決定する
        float angle = Random.Range(0f, 360f);

        while (player != null && currentHP > 0)
        {
            // 弾幕を生成する
            for (int i = 0; i < bulletCount; i++)
            {
                // 弾幕の速度をランダムに決定する
                float speed = Random.Range(2f, 2.5f);
                float angleStep = 360f / bulletCount;
                float bulletAngle = angle + i * angleStep;

                Vector2 velocity = new Vector2(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad)) * speed;

                for (int j = 0; j < bulletSpawners.Length; j++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, bulletSpawners[j].position, Quaternion.identity);
                    bullet.transform.right = velocity.normalized;
                    bullet.GetComponent<Rigidbody2D>().velocity = velocity;
                }
            }

            yield return new WaitForSeconds(bulletFireRate);
        }
    }

    private IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float duration = 0.4f;
        float halfDuration = duration / 3f;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.Lerp(Color.black, originalColor, 0.3f);
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = originalColor;

        isShowingDamage = false;
    }

}