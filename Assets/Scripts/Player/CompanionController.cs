using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : MonoBehaviour
{
    PlayerController player;
    public GameObject bulletPrefab; // Bulletのプレハブ
    public Transform bulletSpawnPoint; // Bulletの発射位置
    public float bulletSpeed = 10f; // 弾の速度
    private float nextFireTime;  // 次に弾を発射できる時刻

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {

        if (player.isPlayerActive && player.isFiring && Time.time > nextFireTime)
        {
            FireBullet();
        }

    }

    // Bulletを発射するメソッド
    public void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        PlayerBulletController bulletController = bullet.GetComponent<PlayerBulletController>();
        float fireRate = bulletController.fireRate;
        int attackPower = bulletController.attackPower;

        nextFireTime = Time.time + fireRate;
    }
}
