using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Stage3Boss : MonoBehaviour
{
    Stage3Manager stage3Manager;
    public int maxHP;
    public float currentHP;

    public GameObject bulletPrefab;
    public GameObject LaserPrefab;
    public GameObject bulletSpawner1;
    public GameObject bulletSpawner2;
    GameObject player;
    public float bulletSpeed = 3f;
    public float fireRate = 1f;  // 1秒に1回弾を発射する
    private float currentAngle = 325f;  // Current angle to fire the bullet
    private float angleStep = 8.33f;  // Angle step to increase/decrease the angle
    private bool isIncreasing = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private BoxCollider2D boxCollider;
    public GameObject MagicCirclePrefab;
    public GameObject explosionPrefab;
    private bool isShowingDamage = false;
    GameManager gameManager;

    void Start()
    {
        currentHP = maxHP;

        stage3Manager = FindObjectOfType<Stage3Manager>();
        gameManager = FindObjectOfType<GameManager>();

        player = FindObjectOfType<PlayerController>().gameObject;
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        StartCoroutine(StartAction());
    }

    void Update()
    {
        // Change the direction if the angle reaches 325 or 225
        if (currentAngle >= 325f)
            isIncreasing = false;
        else if (currentAngle <= 225f)
            isIncreasing = true;

        // Update the current angle
        currentAngle += (isIncreasing ? angleStep - 3.3f : -angleStep + 3.3f);
    }

    IEnumerator StartAction()
    {
        yield return new WaitForSeconds(2f);
        boxCollider.enabled = true;

        StartCoroutine(FireFanShapedBulletsRoutine());
        StartCoroutine(CreateFallObjectsRoutine());
    }

    private IEnumerator FireFanShapedBulletsRoutine()
    {
        while (currentHP != 0)
        {
            yield return new WaitForSeconds(4f);

            for (int i = 0; i <= 11; i++)
            {
                yield return new WaitForSeconds(0.01f);

                float bulletDirX = Mathf.Sin(currentAngle * Mathf.Deg2Rad);  // X-coordinate calculation
                float bulletDirY = Mathf.Cos(currentAngle * Mathf.Deg2Rad);  // Y-coordinate calculation

                Vector3 bulletVector = new Vector3(bulletDirX, bulletDirY);  // The vector of the bullet
                Vector3 bulletMoveDirection = bulletVector.normalized;  // The direction of the bullet movement

                GameObject tmp = Instantiate(bulletPrefab, bulletSpawner2.transform.position, Quaternion.identity);
                tmp.transform.right = bulletMoveDirection;  // Rotate the bullet to face the direction
                tmp.GetComponent<Rigidbody2D>().velocity = bulletMoveDirection * bulletSpeed;

                // Change the direction if the angle reaches 325 or 225
                if (currentAngle >= 325f)
                    isIncreasing = false;
                else if (currentAngle <= 225f)
                    isIncreasing = true;

                // Update the current angle
                currentAngle += (isIncreasing ? angleStep : -angleStep);
            }
        }
    }

    private IEnumerator CreateFallObjectsRoutine()
    {
        while (currentHP != 0)
        {
            yield return new WaitForSeconds(6f);
            GameObject magicCircle = Instantiate(MagicCirclePrefab, bulletSpawner1.transform.position, Quaternion.identity);
            yield return new WaitForSecondsRealtime(1.5f);
            Destroy(magicCircle);

            int randomNumber = Random.Range(1, 101);  // 1から100までのランダムな数値を生成

            if (randomNumber % 2 == 0)
            {
                // 配列が空でないかチェック
                if (stage3Manager.fallObjects.Length > 0)
                {
                    // 偶数の場合の処理
                    Instantiate(stage3Manager.magicCircleToFall, stage3Manager.magicCirclePos[0].position, Quaternion.identity);
                    Instantiate(stage3Manager.magicCircleToFall, stage3Manager.magicCirclePos[2].position, Quaternion.identity);
                    yield return new WaitForSeconds(1f);

                    int randomIndex1 = Random.Range(0, stage3Manager.fallObjects.Length);
                    int randomIndex2 = Random.Range(0, stage3Manager.fallObjects.Length);

                    Instantiate(stage3Manager.fallObjects[randomIndex1], stage3Manager.fallObjectsPos[0].position, Quaternion.identity);
                    Instantiate(stage3Manager.fallObjects[randomIndex2], stage3Manager.fallObjectsPos[2].position, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("GameObjectの配列が空です。");
                }
            }
            else
            {
                if (stage3Manager.fallObjects.Length > 0)
                {
                    // 偶数の場合の処理
                    Instantiate(stage3Manager.magicCircleToFall, stage3Manager.magicCirclePos[1].position, Quaternion.identity);
                    Instantiate(stage3Manager.magicCircleToFall, stage3Manager.magicCirclePos[3].position, Quaternion.identity);
                    yield return new WaitForSeconds(1f);

                    int randomIndex1 = Random.Range(0, stage3Manager.fallObjects.Length);
                    int randomIndex2 = Random.Range(0, stage3Manager.fallObjects.Length);

                    Instantiate(stage3Manager.fallObjects[randomIndex1], stage3Manager.fallObjectsPos[1].position, Quaternion.identity);
                    Instantiate(stage3Manager.fallObjects[randomIndex2], stage3Manager.fallObjectsPos[3].position, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("GameObjectの配列が空です。");
                }
            }
        }
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

        float duration = 0.2f;
        float halfDuration = duration / 2f;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.black;
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = Color.Lerp(Color.black, originalColor, 0.5f);
        yield return new WaitForSeconds(halfDuration);

        spriteRenderer.color = originalColor;

        isShowingDamage = false;
    }
}
