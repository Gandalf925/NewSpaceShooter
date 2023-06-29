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
    private bool canFire = false;
    public GameObject MagicCirclePrefab;

    void Start()
    {
        currentHP = maxHP;

        stage3Manager = FindObjectOfType<Stage3Manager>();

        player = FindObjectOfType<PlayerController>().gameObject;
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        // 5秒後にBulletの発射を開始する
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
        currentAngle += (isIncreasing ? angleStep - 3.3f : -angleStep + 3.3f);
    }

    IEnumerator EnemyRoutine()
    {
        yield return new WaitForSeconds(2f);
        boxCollider.enabled = true;
        canFire = true;

        while (currentHP != 0 && canFire)
        {
            // 弾を発射
            StartCoroutine(FireFanShapedBullets());
            StartCoroutine(CreateFallObjects());
        }

    }

    private IEnumerator FireFanShapedBullets()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i <= 15; i++)
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

    private IEnumerator CreateFallObjects()
    {

        GameObject magicCircle = Instantiate(MagicCirclePrefab, bulletSpawner1.transform.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);
        Destroy(magicCircle);
        yield return new WaitForSeconds(8f);

    }
}
