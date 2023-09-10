using UnityEngine;

public class BossShield : MonoBehaviour
{
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
        }
        else if (other.CompareTag("SpecialBullet"))
        {
            // スペシャル弾丸が接触した場合、何もしない（シールドを貫通）
        }
    }
}