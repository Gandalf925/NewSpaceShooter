using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldImpact : MonoBehaviour
{
    public GameObject impactVfx;
    public GameObject crashVfx;
    public float waitBeforeDestroyImpact = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            // プレイヤーの通常の弾丸が接触した場合
            Rigidbody rb = other.GetComponent<Rigidbody>();

            // ランダムな方向に反射
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 0f)
            );

            rb.velocity = randomDirection.normalized * rb.velocity.magnitude;

            GameObject impact = Instantiate(impactVfx, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
            Destroy(impact, waitBeforeDestroyImpact);
        }

        if (other.CompareTag("SpecialBullet"))
        {
            var crash = Instantiate(crashVfx, new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z), Quaternion.identity);
            Destroy(crash, waitBeforeDestroyImpact);
            Destroy(this.gameObject);
        }
    }
}
