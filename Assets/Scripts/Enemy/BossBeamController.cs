using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeamController : MonoBehaviour
{
    [SerializeField] GameObject explosionPrefab;
    private void Update()
    {
        // 画面外に出た弾を自動的に削除する
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.x < -100 || screenPosition.x > Screen.width + 100
            || screenPosition.y < -100 || screenPosition.y > Screen.height + 100)
        {
            Destroy(gameObject);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}