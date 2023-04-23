using System.Collections;
using UnityEngine;

public class NormalEnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public float spawnRateMin = 0.5f;
    public float spawnRateMax = 1.5f;
    public float spawnDelay = 0f;
    public float spawnDuration = 120f;
    public int firstWaveCount = 20;

    private float spawnTimer = 0f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        yield return new WaitForSeconds(spawnDelay);

        // 最初の1分間は20体の敵を出現させる
        int remainingCount = firstWaveCount;
        while (remainingCount > 0)
        {
            SpawnEnemy();
            remainingCount--;
            yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));
        }

        // 残りの1分間は約10体程度をまばらに出現させる
        while (spawnTimer < spawnDuration)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(Random.Range(spawnRateMin, spawnRateMax));
            spawnTimer += Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        Vector3 spawnPosition = spawnPoints[spawnPointIndex].position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        float moveSpeed = Random.Range(2f, 4f);
        enemy.GetComponent<NormalEnemy>().moveSpeed = moveSpeed;
    }
}