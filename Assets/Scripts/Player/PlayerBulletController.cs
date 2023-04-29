using System.Collections;
using UnityEngine;

public enum BulletType
{
    NormalBullet,
    RapidBullet,
    BigBullet,
}

public class PlayerBulletController : MonoBehaviour
{
    public BulletType bulletType;
    public float speed;
    public float fireRate;
    public int attackPower;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        switch (bulletType)
        {
            case BulletType.NormalBullet:
                speed = 10f;
                fireRate = 0.4f;
                attackPower = 2;
                break;
            case BulletType.RapidBullet:
                speed = 10f;
                fireRate = 0.2f;
                attackPower = 1;
                break;
            case BulletType.BigBullet:
                speed = 8f;
                fireRate = 1f;
                attackPower = 3;
                break;
        }

        rb.velocity = transform.right * speed;

    }

    private void Update()
    {
        // 画面外に出た弾を自動的に削除する
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < -100 || screenPosition.x > Screen.width + 100
            || screenPosition.y < -100 || screenPosition.y > Screen.height + 100)
        {
            Destroy(gameObject);
        }
    }
}