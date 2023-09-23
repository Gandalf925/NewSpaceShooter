using UnityEngine;

public class PlayerShootController : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject specialbulletPrefab;
    public float shootSpeedZ = 20.0f;
    public float fireRate = 1.0f;

    public GameObject iconPrefab;  // アイコンのプレファブをインスペクタからアタッチ
    private GameObject currentIcon;  // 現在表示されているアイコン

    private float nextFire = 0.0f;

    Player3DController player3DController;

    private void Start()
    {
        player3DController = GetComponent<Player3DController>();
    }

    void Update()
    {
        ShowIcon();
        if (player3DController.canShoot)
        {

            if (player3DController.isSpecialGun)
            {
                if (Time.time > nextFire && Input.GetKeyUp(KeyCode.Mouse0))
                {
                    nextFire = Time.time + 1.0f / fireRate;
                    ShootSpecialBullet();
                }
                if (Time.time > nextFire && Input.GetKey(KeyCode.Space))
                {
                    nextFire = Time.time + 1.0f / fireRate;
                    ShootSpecialBullet();
                }
            }
            else
            {
                if (Time.time > nextFire && Input.GetKey(KeyCode.Mouse0))
                {
                    nextFire = Time.time + 1.0f / fireRate;
                    ShootBullet();
                }

                if (Time.time > nextFire && Input.GetKey(KeyCode.Space))
                {
                    nextFire = Time.time + 1.0f / fireRate;
                    ShootBullet();
                }
            }
        }

    }

    void ShowIcon()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Boss"))
            {
                if (currentIcon == null)
                {
                    currentIcon = Instantiate(iconPrefab, hit.point, Quaternion.identity);
                }
                else
                {
                    currentIcon.transform.position = hit.point;
                }
            }
            else
            {
                if (currentIcon != null)
                {
                    Destroy(currentIcon);
                    currentIcon = null;
                }
            }
        }
        else
        {
            // Rayが何にも当たらない場合、アイコンを削除
            if (currentIcon != null)
            {
                Destroy(currentIcon);
                currentIcon = null;
            }
        }
    }


    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 shootDirection = new Vector3(0.0f, 0.0f, shootSpeedZ);
        player3DController.PlayShootSE();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.TransformDirection(shootDirection);
        }
    }

    void ShootSpecialBullet()
    {
        GameObject bullet = Instantiate(specialbulletPrefab, transform.position, Quaternion.identity);
        Vector3 shootDirection = new Vector3(0.0f, 0.0f, shootSpeedZ);
        player3DController.PlayShootSE();
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = transform.TransformDirection(shootDirection);
        }
    }
}