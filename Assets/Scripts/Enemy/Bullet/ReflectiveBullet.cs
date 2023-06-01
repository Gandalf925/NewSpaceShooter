using UnityEngine;

public class ReflectiveBullet : MonoBehaviour
{
    public float speed = 10f;  // Bulletの移動速度
    public float rotationSpeed = 20f; // 回転速度
    [SerializeField] GameObject explosionPrefab;

    private Rigidbody2D rb;
    private int reflectCount = 0;  // 反射回数のカウント

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = rb.velocity.normalized * speed;  // Bulletを移動速度に合わせて正しい進行方向に修正
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

        // 画面外に出た弾を自動的に削除する
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < -100 || screenPosition.x > Screen.width + 100
            || screenPosition.y < -100 || screenPosition.y > Screen.height + 100)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            if (reflectCount < 3)  // 反射回数が3回未満の場合のみ反射させる
            {
                // 衝突した壁の法線ベクトルを取得して反射方向を計算
                Vector2 reflectDirection = Vector2.Reflect(rb.velocity.normalized, collision.transform.up);

                // 反射方向をBulletの移動方向に設定
                rb.velocity = reflectDirection * speed;

                reflectCount++;
            }
            else
            {
                // 反射回数が3回以上ならBulletを破棄するなどの処理を行う
                Destroy(gameObject);
            }
        }
    }
}
