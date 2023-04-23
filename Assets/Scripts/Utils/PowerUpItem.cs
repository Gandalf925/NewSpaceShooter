using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    public enum PowerupType
    {
        Small,  // 小アイテム
        Large,  // 大アイテム
    }

    public PowerupType powerupType;  // アイテムの種類
    public float moveSpeed = 6f;  // 移動速度
    public float rotateSpeed = 180f;  // 回転速度
    public float targetAngle = 25f; // 目標角度
    public float rightMoveDistance = 2.5f; // 右に移動する距離
    public float leftMoveSpeed = 1f; // 左に移動する速度

    private Rigidbody2D rb;
    private Vector3 targetPosition;
    private Vector3 startPosition;
    private bool isMovingToTarget = false;
    private bool isMovingToLeft = false;
    GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;

        // 目標位置をランダムに決定する
        float angle = Random.Range(-targetAngle, targetAngle) * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0f);
        targetPosition = startPosition + direction * rightMoveDistance;
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        if (!isMovingToTarget)
        {
            // 目標位置へ移動する
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * moveSpeed;

            // 目標位置に到達したら、減速して反対側に移動する
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                rb.velocity *= 0.5f;
                isMovingToTarget = true;
            }
        }
        else
        {
            // 左に移動する
            if (!isMovingToLeft)
            {
                rb.velocity = Vector3.left * leftMoveSpeed;
                if (rb.velocity.x < 0f)
                {
                    isMovingToLeft = true;
                }
            }
            else
            {
                // 画面外に出たらアイテムを削除する
                if (transform.position.x < -10f || transform.position.y < -6f || transform.position.y > 6f)
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    // アイテムの種類を設定する処理
    public void SetItemType(PowerupType type)
    {
        powerupType = type;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // アイテムの種類によってスコアを変更する
            if (powerupType == PowerupType.Small)
            {
                gameManager.UpdateScore(1);
            }
            else if (powerupType == PowerupType.Large)
            {
                gameManager.UpdateScore(5);
            }

            Destroy(gameObject);
        }
    }
}