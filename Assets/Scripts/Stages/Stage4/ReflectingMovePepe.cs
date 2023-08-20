using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ReflectingMovePepe : MonoBehaviour
{
    public int maxHP = 1;
    private float currentHP;
    public GameObject explosionPrefab;
    public Transform startPos;
    public Transform stopPos;
    public float moveSpeed = 3f;
    public float waitTime = 1f;
    public int maxReflections = 10;
    public float reflectionDelay = 3f;

    private int reflectionsCount = 0;
    private Vector2 direction;
    private Rigidbody2D rb;
    private bool hasHitWall = false;
    public GameObject player;

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
        rb = GetComponent<Rigidbody2D>();
        rb.simulated = false;

        currentHP = maxHP;
        startPos = GameObject.FindGameObjectWithTag("EliteStartPos").transform;
        stopPos = GameObject.FindGameObjectWithTag("EliteStopPos").transform;
        image = GetComponent<Image>();
        originalColor = image.color;
        player = GameObject.FindWithTag("Player");
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // Move from startPos to stopPos in 1 second
        transform.position = startPos.position;
        transform.DOMove(stopPos.position, 1f).OnComplete(StartMoving);
    }

    void StartMoving()
    {
        // Wait for 1 second
        Invoke(nameof(BeginReflection), waitTime);
    }

    void BeginReflection()
    {
        if (reflectionsCount >= maxReflections)
        {
            reflectionsCount = 0;
            // Wait for reflectionDelay seconds before starting again
            Invoke(nameof(StartMoving), reflectionDelay);
            return;
        }

        rb.simulated = true;

        // Random direction
        float randomAngle = Random.Range(0, 360) * Mathf.Deg2Rad;
        direction = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }

    void FixedUpdate()
    {
        if (transform.position.x < -12 || transform.position.x > 12 || transform.position.y < -10 || transform.position.y > 10)
        {
            Destroy(gameObject);
        }

        else if (hasHitWall)
        {
            hasHitWall = false;
            if (reflectionsCount >= maxReflections)
            {
                rb.simulated = false;
                BeginReflection();
            }
        }
        else
        {
            rb.velocity = direction * moveSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("WallNoDamage"))
        {
            // Get the normal from the WallNormal component
            Vector2 normal = other.GetComponent<WallNormal>().normal;

            // Reflect off wall
            direction = Vector2.Reflect(direction, normal);
            reflectionsCount++;
            hasHitWall = true;
        }

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