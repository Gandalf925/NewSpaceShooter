using UnityEngine;
using DG.Tweening;

public class LaserController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject laserStartVFX;
    public GameObject laserEndVFX;
    public BoxCollider2D boxCollider;

    private Transform laserStartBeamFlash;
    private Transform laserEndBeamFlash;

    private void Awake()
    {
        // BeamFlashを取得
        laserStartBeamFlash = laserStartVFX.transform.Find("BeamFlash");
        laserEndBeamFlash = laserEndVFX.transform.Find("BeamFlash");

        // 初期状態でBoxColliderを無効化
        boxCollider.enabled = false;
    }

    private void Start()
    {
        // LineRendererの初期位置を設定
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);

        // laserEndVFXを移動させる処理
        Vector3 endPosition = new Vector3(-20, 0, 0);

        // laserEndVFXのローカル座標を使用して移動
        laserEndVFX.transform.DOLocalMove(endPosition, 0.7f).OnStart(() =>
        {
            // BoxColliderを有効化
            boxCollider.enabled = true;
        }).OnUpdate(() =>
        {
            // LineRendererの終点をlaserEndVFXのローカル位置に更新
            lineRenderer.SetPosition(1, laserEndVFX.transform.localPosition);

            // BoxColliderの位置とサイズを調整
            float laserLength = laserEndVFX.transform.localPosition.x;
            boxCollider.size = new Vector2(Mathf.Abs(laserLength), boxCollider.size.y);
            boxCollider.offset = new Vector2(laserLength / 2, 0);
        });

        // 5秒後にLaserを消す処理を呼び出す
        Invoke(nameof(DestroyLaser), 4f);
    }

    private void DestroyLaser()
    {
        // BeamFlashのスケールを0にする
        laserStartBeamFlash.DOScale(Vector3.zero, 1f);
        laserEndBeamFlash.DOScale(Vector3.zero, 1f);

        // LineのWidthを0にする
        DOTween.To(() => lineRenderer.startWidth, x => lineRenderer.startWidth = x, 0f, 1f);
        DOTween.To(() => lineRenderer.endWidth, x => lineRenderer.endWidth = x, 0f, 1f).OnComplete(() =>
        {
            // 処理が終わったらLaserをDestroy
            Destroy(gameObject);
        });
    }
}
