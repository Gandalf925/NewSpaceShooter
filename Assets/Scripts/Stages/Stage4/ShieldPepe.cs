using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ShieldPepe : MonoBehaviour
{
    public int maxHP = 1;
    private float currentHP;
    public float moveSpeed = 2f;
    public float bulletSpeed = 10f;
    public float bulletFireRate = 2f;
    public GameObject bulletPrefab;
    public GameObject player;
    public GameObject explosionPrefab;
    public Transform bulletSpawnPoint;

    private bool isShowingDamage = false;
    private Image image;
    private Color originalColor;
    GameManager gameManager;

    [Header("Sound")]
    BGMManager soundManager;

    void Start()
    {
        currentHP = maxHP;
        image = GetComponent<Image>();
        originalColor = image.color;
        player = GameObject.FindWithTag("Player");
        soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

        // 弾の発射
        StartCoroutine(FireRoutine());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
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
                player.GetComponent<PlayerController>().PlayExplosionSE();


                Destroy(explosion, 0.5f);

                Destroy(gameObject);
            }

            Destroy(other.gameObject);
        }
    }

    private IEnumerator FireRoutine()
    {
        yield return new WaitForSeconds(4f);



        while (player != null)
        {

            float randomValue = Random.Range(0f, 1f);
            yield return new WaitForSeconds(randomValue);

            // 発射時に現在のPlayerの位置を取得する
            if (player != null)
            {
                Vector3 targetPosition = player.transform.position;

                GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bullet.transform.right = (targetPosition - bulletSpawnPoint.position).normalized;
                bullet.GetComponent<Rigidbody2D>().velocity = bullet.transform.right * bulletSpeed;

                // 現在のPlayerの位置に向かって発射するようにする
                bullet.GetComponent<Rigidbody2D>().velocity = (targetPosition - bulletSpawnPoint.position).normalized * bulletSpeed;
                yield return new WaitForSeconds(bulletFireRate);
            }
            else
            {
                yield return null;
            }
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

}
