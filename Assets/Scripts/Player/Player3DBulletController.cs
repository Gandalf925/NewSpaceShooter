using UnityEngine;

public class Player3DBulletController : MonoBehaviour
{
    public GameObject bulletPrefab;  // BulletPrefabをインスペクターからアタッチ
    public float shootSpeedZ = 20.0f;  // 弾のZ軸方向の発射速度
    public float shootSpeedY = 5.0f;  // 弾のY軸方向の発射速度（インスペクターで調整可能）

    // Update is called once per frame
    void Update()
    {
        // ここではSpaceキーを押すと発射するようにしています。
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ShootBullet();
        }
    }

    void ShootBullet()
    {
        // BulletPrefabから新しい弾をインスタンス化
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        // 発射方向を計算（Z軸方向に移動しながらY軸方向に少し上へ）
        Vector3 shootDirection = new Vector3(0.0f, shootSpeedY, shootSpeedZ);

        // 弾に速度を与える
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.TransformDirection(shootDirection);
        }
    }
}
