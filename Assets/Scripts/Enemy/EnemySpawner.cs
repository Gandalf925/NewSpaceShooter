using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemySpawner : MonoBehaviour
{
    public GameObject normalEnemyPrefab;
    public GameObject radialEnemyPrefab;
    public Transform[] normalEnemySpawnPoints;
    public Transform radialEnemySpawnPoint;

    private WaitForSeconds spawnDelay = new WaitForSeconds(5f);

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            int normalEnemyCount = Random.Range(3, 7);
            for (int i = 0; i < normalEnemyCount; i++)
            {
                GameObject normalEnemy = Instantiate(normalEnemyPrefab, normalEnemySpawnPoints[Random.Range(0, normalEnemySpawnPoints.Length)].position, Quaternion.identity);
                yield return new WaitForSeconds(1f / Random.Range(4, 7));
            }

            yield return spawnDelay;

            if (Random.Range(0, 2) == 0)
            {
                GameObject radialEnemy = Instantiate(radialEnemyPrefab, radialEnemySpawnPoint.position, Quaternion.identity);
            }
            else
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject radialEnemy = Instantiate(radialEnemyPrefab, radialEnemySpawnPoint.position + new Vector3(Random.Range(-2f, 2f), Random.Range(-2f, 2f), 0f), Quaternion.identity);
                    radialEnemy.transform.DOMove(radialEnemySpawnPoint.position, 1.5f).SetEase(Ease.OutBounce);
                }
            }

            yield return new WaitForSeconds(Random.Range(1f, 3f));
        }
    }
}