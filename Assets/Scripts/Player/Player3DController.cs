using UnityEngine;

public class Player3DController : MonoBehaviour
{
    public float speed = 5.0f; // 移動速度
    public float stopDistance = 0.1f; // この距離以下になったら停止
    private Rigidbody rb; // Rigidbodyコンポーネント
    private float fixedZPosition; // 固定されたZ座標

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedZPosition = transform.position.z; // 初期のZ座標を保存
    }

    void FixedUpdate()
    {
        // Z座標を固定
        Vector3 newPosition = transform.position;
        newPosition.z = fixedZPosition;
        transform.position = newPosition;

        if (!Input.GetMouseButton(0))
        {
            rb.velocity = Vector3.zero;
            return;
        }

        Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10); // Z値はカメラからの距離

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
        mouseWorldPos.z = fixedZPosition; // Z座標も固定

        Vector3 targetPosition = mouseWorldPos + new Vector3(1, 1, 0);
        Vector3 diff = targetPosition - transform.position;

        if (diff.magnitude < stopDistance)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            rb.velocity = diff.normalized * speed;
        }
    }
}
