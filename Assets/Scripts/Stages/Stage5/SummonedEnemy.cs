using System.Collections;
using UnityEngine;
using DG.Tweening;

public class SummonedEnemy : MonoBehaviour
{
    public GameObject bulletPrefab; // 弾のプレファブ
    public float fireDelay = 1.0f; // 発射までの遅延（秒）

    private Transform playerTransform;

    void Start()
    {
        // PlayerのTransformを取得
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        // 初期位置を保存
        Vector3 initialPosition = transform.position;

        // ゆらゆら動くアニメーション
        float xJitter = Random.Range(-1f, 1f);
        float yJitter = Random.Range(-1f, 1f);

        transform.DOShakePosition(3, new Vector3(xJitter, yJitter, 0), 10, 90, false, true).SetLoops(-1, LoopType.Yoyo);

        // 1秒後に発射を開始
        StartCoroutine(StartFiring());
    }

    IEnumerator StartFiring()
    {
        yield return new WaitForSeconds(fireDelay);

        while(true)
        {
            FireBullet();
            yield return new WaitForSeconds(fireDelay);
        }
    }

    void FireBullet()
    {
        // 弾を生成して、プレイヤーの方向に発射する処理
        GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        Vector3 direction = (playerTransform.position - transform.position).normalized;
        bullet.GetComponent<Rigidbody>().velocity = direction * 10; // 速度は適宜調整
    }
}
