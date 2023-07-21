using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsMoveController : MonoBehaviour
{

    public float moveSpeed = 5f; // 移動スピード

    private void Update()
    {
        // 左に移動させる
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);

        // 画面左の外に出たらオブジェクトを削除する
        if (transform.position.x < -30)
        {
            // randomPrefabSpawner.
        }
    }
}
