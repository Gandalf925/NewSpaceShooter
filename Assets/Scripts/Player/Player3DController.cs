using UnityEngine;

public class Player3DController : MonoBehaviour
{
    public float speed = 5.0f; // 移動速度
    public float touchSensitivity = 0.1f; // タッチ感度（移動を開始するための最小距離）
    private Rigidbody rb; // Rigidbody コンポーネント
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

        // オフセットを加えて目標位置を計算
        float offsetX = 4f;
        float offsetY = 0.3f;
        Vector3 targetPosition = mouseWorldPos + new Vector3(offsetX, offsetY, 0);

        // 現在位置と目標位置との差分を計算
        Vector3 diff = targetPosition - transform.position;
        diff.z = 0; // Z座標の差分は0に

        // 差分ベクトルが一定値以上なら移動、それ以下なら停止
        if (diff.magnitude > touchSensitivity)
        {
            rb.velocity = diff.normalized * speed;
        }
        else
        {
            transform.position = targetPosition; // 目標位置に移動
            rb.velocity = Vector3.zero; // 速度をゼロに
        }
    }
}
