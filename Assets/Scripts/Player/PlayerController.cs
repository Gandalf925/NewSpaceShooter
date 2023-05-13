using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    public int speed = 5;  // キャラクターの移動速度
    public float touchSensitivity = 0.1f;

    public GameObject[] bulletPrefabs;  // 弾のプレハブの配列
    public Transform bulletSpawnPoint;  // 弾の発射位置
    public Image touchCircle;

    public GameObject explosionEffect;  // 爆発エフェクトのプレハブ

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private float nextFireTime;  // 次に弾を発射できる時刻
    private int selectedBulletIndex = 0;  // 選択された弾のインデックス

    private float invincibleTime = 3f;
    private float invincibleTimer = 0f;
    private bool isInvincible = false;
    private bool isFiring = false;
    private Image playerImage;

    private int previousPowerupPoint;

    GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        playerImage = GetComponent<Image>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        touchCircle.gameObject.SetActive(true);

        StartCoroutine(BlinkTouchCircle());
    }

    private void Update()
    {
        // キャラクターの移動(マウスとスマホ)
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 diff = mousePos - transform.position;
            diff.z = 0;

            // 調整量を加算する
            float offsetX = 4f;
            float offsetY = 0.3f;
            diff += new Vector3(offsetX, offsetY, 0);


            // 差分ベクトルが一定値以上であれば移動する
            if (diff.magnitude > touchSensitivity)
            {
                rb.velocity = diff.normalized * speed;
            }
            else
            {
                rb.velocity = Vector2.zero;
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        // キャラクターの移動（PC）
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            float x = (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)) ? 1f :
                      (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) ? -1f : 0f;
            float y = (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)) ? 1f :
                      (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) ? -1f : 0f;

            // Shiftキーが押されている場合は速度を半分にする
            float currentSpeed = speed;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                currentSpeed *= 0.5f;
            }

            Vector2 movement = new Vector2(x, y);
            rb.velocity = movement * currentSpeed;
        }


        // 攻撃（弾の発射）
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBulletIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBulletIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedBulletIndex = 2;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isFiring = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            isFiring = false;
        }

        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            isFiring = true;
        }
        else if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space))
        {
            isFiring = false;
        }

        if (isFiring && Time.time > nextFireTime)
        {
            // 選択された弾のプレハブを取得
            GameObject bulletPrefab = bulletPrefabs[selectedBulletIndex];

            // 弾のプレハブをインスタンス化
            GameObject bulletInstance = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);

            // BulletControllerの発射間隔、攻撃力を取得
            PlayerBulletController bulletController = bulletInstance.GetComponent<PlayerBulletController>();
            float fireRate = bulletController.fireRate;
            int attackPower = bulletController.attackPower;

            //パワーアップ時の処理
            if (gameManager.powerupCount >= 1)
            {
                float angleLeft = -15f;
                float angleRight = 15f;

                GameObject bulletInstanceLeft = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                bulletInstanceLeft.transform.rotation = Quaternion.Euler(0f, 0f, angleLeft);
                bulletInstanceLeft.GetComponent<PlayerBulletController>().fireRate = fireRate;
                bulletInstanceLeft.GetComponent<PlayerBulletController>().attackPower = attackPower;

                GameObject bulletInstanceRight = Instantiate(bulletPrefab, bulletSpawnPoint.position, Quaternion.identity);
                bulletInstanceRight.transform.rotation = Quaternion.Euler(0f, 0f, angleRight);
                bulletInstanceRight.GetComponent<PlayerBulletController>().fireRate = fireRate;
                bulletInstanceRight.GetComponent<PlayerBulletController>().attackPower = attackPower;
            }

            // 次に弾を発射できる時刻を更新
            nextFireTime = Time.time + fireRate;

            // 衝突判定を一時的に無効化する
            col.enabled = false;

            // 弾が発射された後、衝突判定を再度有効化する
            Invoke("EnableCollider", 0.1f);
        }

        // 画面外に出られないようにする
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(transform.position.x, -8f, 8f);
        clampedPosition.y = Mathf.Clamp(transform.position.y, -4.5f, 4.5f);
        transform.position = clampedPosition;

        // 弾の種類を変更する
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            selectedBulletIndex = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            selectedBulletIndex = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            selectedBulletIndex = 2;
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

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 敵のBulletがPlayerに当たった場合

        if (isInvincible) return;

        if (other.CompareTag("EnemyBullet") || other.CompareTag("Enemy"))
        {
            gameManager.UpdateLives(-1);
            PowerDown();

            DisableCollider();
            isInvincible = true;

            if (gameManager.lives <= 0)
            {
                // 爆発エフェクトを生成し、プレイヤーの位置に設定する
                GameObject effectInstance = Instantiate(explosionEffect, transform.position, Quaternion.identity);
                Destroy(effectInstance, 1f);  // エフェクトが終了したら破棄する

                // プレイヤーを破棄する
                Destroy(gameObject);
            }
        }
    }


    public void Powerup()
    {
        gameManager.powerupCount += 1;
        speed += 1;
    }

    private void PowerDown()
    {
        if (gameManager.powerupCount >= 1)
        {
            gameManager.powerupCount -= 1;
            speed -= 1;
            if (gameManager.powerupCount < 0)
            {
                gameManager.powerupCount = 0;
            }
        }
    }

    IEnumerator BlinkTouchCircle()
    {
        yield return new WaitForSecondsRealtime(3f);

        touchCircle.DOFade(0f, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        touchCircle.DOFade(1f, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        touchCircle.DOFade(0f, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        touchCircle.DOFade(1f, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        touchCircle.DOFade(0f, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);
        touchCircle.DOFade(1f, 0.3f);
        yield return new WaitForSecondsRealtime(0.3f);

        touchCircle.gameObject.SetActive(false);

    }

    public void EnableCollider()
    {
        col.enabled = true;
    }
    public void DisableCollider()
    {
        col.enabled = false;
    }
}
