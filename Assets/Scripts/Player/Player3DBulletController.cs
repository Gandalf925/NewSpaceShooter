using UnityEngine;

public class Player3DBulletController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootSpeedZ = 20.0f;
    public float shootSpeedY = 5.0f;
    public float fireRate = 1.0f;  // 1秒あたりの発射回数

    [SerializeField] GameObject explosionPrefab;

    private float nextFire = 0.0f;  // 次に弾を発射できるタイミング

    // Update is called once per frame
    void Update()
    {
        // 現在の時間が次の発射時間以上かつ、マウス左クリックが押された場合に発射
        if (Time.time > nextFire && Input.GetKey(KeyCode.Mouse0))
        {
            nextFire = Time.time + 1.0f / fireRate;  // 次に発射できる時間を更新
            ShootBullet();
        }

        if (transform.position.z > 75)
        {
            Destroy(gameObject);
        }
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 shootDirection = new Vector3(0.0f, shootSpeedY, shootSpeedZ);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.TransformDirection(shootDirection);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(explosion, 0.5f);
            Destroy(gameObject);
        }
    }
}
