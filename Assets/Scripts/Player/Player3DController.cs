using UnityEngine;

public class Player3DController : MonoBehaviour
{
    public float speed = 5.0f;
    public float touchSensitivity = 0.1f;
    private Rigidbody rb;
    public GameObject iconPrefab;
    private float fixedZPosition;
    public Camera mainCamera; // MainCameraをインスペクタからアタッチする
    public Vector3 cameraOffset; // カメラがプレイヤーからどれだけ離れているか

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fixedZPosition = transform.position.z;

        // カメラの初期位置を設定（オプション）
        mainCamera.transform.position = transform.position + cameraOffset;
    }

    void FixedUpdate()
    {
        Vector3 newPosition = transform.position;
        newPosition.z = fixedZPosition;
        transform.position = newPosition;

        if (!Input.GetMouseButton(0))
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            Vector3 mouseScreenPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = fixedZPosition;

            float offsetX = 6f;
            float offsetY = 1f;
            Vector3 targetPosition = mouseWorldPos + new Vector3(offsetX, offsetY, 0);

            Vector3 diff = targetPosition - transform.position;
            diff.z = 0;

            if (diff.magnitude > touchSensitivity)
            {
                if (rb.isKinematic)
                {
                    rb.isKinematic = false;  // 物理演算を有効にする
                }
                rb.velocity = diff.normalized * speed;
            }
            else
            {
                if (!rb.isKinematic)
                {
                    rb.isKinematic = true;  // 物理演算を無効にする
                }
                transform.position = targetPosition;
                rb.isKinematic = false;  // すぐに物理演算を有効に戻す
            }
        }

        // プレイヤーの位置を制限する
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, -20f, 20f);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, 22f, 27f);
        clampedPosition.z = fixedZPosition;  // Z座標も固定

        // 修正された位置をプレイヤーに適用する
        transform.position = clampedPosition;

        // カメラをプレイヤーに追随させる
        mainCamera.transform.position = transform.position + cameraOffset;
    }
}
