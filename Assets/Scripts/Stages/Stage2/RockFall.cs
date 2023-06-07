using UnityEngine;
using System.Collections;
using DG.Tweening;

public class RockFall : MonoBehaviour
{
    public float warningDuration = 1f;  // 落下の予兆の表示時間
    public float approachDistance = 5f;  // 岩がプレイヤーに接近する距離
    private float screenBottomMargin = -10f;  // 画面下のマージン

    private bool isPlayerApproaching = false;  // プレイヤーが接近中かどうかのフラグ
    private Vector3 initialPosition;  // 岩の初期位置

    private Transform playerTransform;  // プレイヤーの位置情報

    private Vector3 currentPlayerPosition;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = transform.position;
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            currentPlayerPosition = playerTransform.position;
        }

        if (!isPlayerApproaching && playerTransform != null)
        {
            // プレイヤーとの距離が接近距離以下になったら攻撃を開始
            float distanceToPlayer = Mathf.Abs(currentPlayerPosition.x - transform.position.x);
            if (distanceToPlayer <= approachDistance)
            {
                isPlayerApproaching = true;
                StartRockAttack();
            }
        }

        if (transform.position.y < screenBottomMargin)
        {
            Destroy(gameObject);
        }
    }

    private void StartRockAttack()
    {
        StartCoroutine(EnableGravityAndFall());
    }

    private IEnumerator EnableGravityAndFall()
    {
        yield return new WaitForSeconds(warningDuration);  // 落下の予兆の表示時間を待機

        isPlayerApproaching = true;  // プレイヤーが接近中フラグを設定

        // 落下の予兆を示す振動アニメーション
        transform.DOShakePosition(1f, strength: new Vector3(0.3f, 0f, 0f), vibrato: 20, fadeOut: false);

        // Rigidbody2Dの重力を有効にして岩を落下させる
        Rigidbody2D rockRigidbody = GetComponent<Rigidbody2D>();
        rockRigidbody.gravityScale = 1f;
    }

    private void LateUpdate()
    {
        // プレイヤーのRectTransformの位置をワールド座標に変換
        Vector3 playerWorldPosition = playerTransform.TransformPoint(Vector3.zero);

        // 岩の位置とプレイヤーの位置の距離判定をワールド座標で行う
        float distance = Vector3.Distance(transform.position, playerWorldPosition);
        if (!isPlayerApproaching && distance <= approachDistance)
        {
            isPlayerApproaching = true;
            StartRockAttack();
        }

    }
}