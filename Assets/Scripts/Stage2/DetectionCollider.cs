using System;
using System.Collections.Generic;
using UnityEngine;

public class DetectionCollider : MonoBehaviour
{
    public List<RollingEnemy> rollingEnemies = new List<RollingEnemy>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // 配列内のRollingEnemyからDestroyされたものを削除する
            for (int i = rollingEnemies.Count - 1; i >= 0; i--)
            {
                if (rollingEnemies[i] == null)
                {
                    rollingEnemies.RemoveAt(i);
                }
            }

            // 配列内の残りのRollingEnemyに対してStartMovingを起動する
            foreach (RollingEnemy enemy in rollingEnemies)
            {
                enemy.StartMoving();
            }
        }
    }
}