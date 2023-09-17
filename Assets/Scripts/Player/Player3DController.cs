using System.Collections;
using UnityEngine;

public class Player3DController : MonoBehaviour
{
    public float speed = 5.0f;
    public float touchSensitivity = 0.1f;
    private Rigidbody rb;
    private Collider col;
    public GameObject iconPrefab;
    private float fixedZPosition;
    public Camera mainCamera; // MainCameraをインスペクタからアタッチする
    public Vector3 cameraOffset; // カメラがプレイヤーからどれだけ離れているか

    public GameObject explosionEffect;  // 爆発エフェクトのプレハブ

    public bool isActive = true;
    private bool isInvincible = false;
    private float invincibleTime = 3f;
    private float invincibleTimer = 0f;

    private SpriteRenderer playerImage;

    public bool isSpecialGun = false;
    public bool canAttack = false;
    public bool canShoot = true;
    public GameObject SpecialBullet;

    GameManager gameManager;

    PlayerShootController playerShootController;

    [Header("Audio")]
    public AudioSource seSource;
    public AudioClip warningSE;
    public AudioClip explosionSE;
    public AudioClip chargingSE;

    [SerializeField] AudioClip shootSE;
    AudioSource source;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        playerImage = GetComponent<SpriteRenderer>();
        fixedZPosition = transform.position.z;

        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // カメラの初期位置を設定（オプション）
        mainCamera.transform.position = transform.position + cameraOffset;


    }

    void Update()
    {

        float horizontalInput = Input.GetAxis("Horizontal"); // ADキーまたは左右矢印キー
        float verticalInput = Input.GetAxis("Vertical"); // WSキーまたは上下矢印キー

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0).normalized;

        if (direction != Vector3.zero)
        {
            rb.velocity = direction * speed;
        }

        if (!Input.GetMouseButton(0))
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

            float offsetX = 6f;
            float offsetY = 1f;
            Vector3 targetPosition = mouseWorldPos + new Vector3(offsetX, offsetY, 0);

            Vector3 diff = targetPosition - transform.position;
            diff.z = 0;


            Vector3 currentDirection = rb.velocity.normalized;
            Vector3 newDirection = diff.normalized;
            float angle = Vector3.Angle(currentDirection, newDirection);

            float currentSpeed = speed;

            // 現在の進行方向と逆側の角度（±30度）にドラッグされたら、スピードを遅くする
            if (Mathf.Abs(angle - 180) <= 60 && diff.magnitude >= touchSensitivity)
            {
                currentSpeed = speed / 10.0f;
            }

            if (diff.magnitude > touchSensitivity)
            {
                rb.velocity = newDirection * currentSpeed;
            }
            else
            {
                rb.velocity = Vector3.zero;
            }
        }

        if (isInvincible)
        {
            invincibleTimer += Time.deltaTime;
            float alpha = Mathf.PingPong(Time.time * 5f, 1f);
            playerImage.color = new Color(1f, 1f, 1f, alpha);

            if (invincibleTimer >= invincibleTime)
            {
                playerImage.color = new Color(1f, 1f, 1f, 1f);
                isInvincible = false;
                EnableCollider();
                invincibleTimer = 0f;
            }

        }

        if (isSpecialGun && !canAttack)
        {
            canAttack = true;
            Debug.Log("SpecialGun");
        }

        // プレイヤーの位置を制限する
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -25f, 25f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 22f, 35f);
        clampedPosition.z = fixedZPosition;  // Z座標も固定

        // 修正された位置をプレイヤーに適用する
        transform.position = clampedPosition;

        // カメラをプレイヤーに追随させる
        mainCamera.transform.position = transform.position + cameraOffset;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isInvincible) return;

        if (other.CompareTag("EnemyBullet") || other.CompareTag("ChargeFire") || other.CompareTag("Asteroids"))
        {
            TakeDamage();

            if (gameManager.lives <= 0)
            {
                Death();
            }
        }
    }

    public void TakeDamage()
    {
        GameObject effectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effectInstance, 1f);  // エフェクトが終了したら破棄する
        gameManager.UpdateLives(-1);
        DisableCollider();
        isInvincible = true;
    }

    public void Death()
    {
        PlayExplosionSE();
        // 爆発エフェクトを生成し、プレイヤーの位置に設定する
        GameObject effectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(effectInstance, 1f);  // エフェクトが終了したら破棄する

        isActive = false;
        // プレイヤーを破棄する
        Destroy(gameObject);
    }

    public void DisableShooting()
    {
        canShoot = false;
    }

    public void EnableShooting()
    {
        canShoot = true;
    }

    public void EnableCollider()
    {
        col.enabled = true;
    }
    public void DisableCollider()
    {
        col.enabled = false;
    }

    public void PlayShootSE()
    {
        source.PlayOneShot(explosionSE);
    }

    public void PlayExplosionSE()
    {
        seSource.clip = explosionSE;
        seSource.pitch = 1.2f;
        seSource.PlayOneShot(explosionSE);
    }

    public IEnumerator PlayWarningSE(float duration)
    {
        seSource.clip = warningSE;
        seSource.pitch = 0.8f;
        seSource.PlayOneShot(warningSE);
        yield return new WaitForSeconds(duration);

        seSource.Stop();
    }

}
