using System.Collections;
using UnityEngine;

public class EnemySpawnerStage1 : MonoBehaviour
{
    public GameObject nomalEnemyPrefab;
    public GameObject radialEnemyPrefab;
    [Header("NormalEnemyPosition")]
    public Transform spawnPositionTop;
    public Transform[] spawnPositionsMiddle;
    public Transform spwanPositionBottom;

    [Header("RadialEnemyPosition")]
    public Transform radialPositionTop;
    public Transform radialPositionMiddle;
    public Transform radialPositionBottom;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        // Game Start
        yield return new WaitForSecondsRealtime(3f);

        // Topから4体出現
        SpawnNormalEnemy(spawnPositionTop);
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnNormalEnemy(spawnPositionTop);
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnNormalEnemy(spawnPositionTop);
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnNormalEnemy(spawnPositionTop);
        yield return new WaitForSecondsRealtime(0.5f);


        yield return new WaitForSecondsRealtime(5f);

        // Bottmeから4体出現
        SpawnNormalEnemy(spwanPositionBottom);
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnNormalEnemy(spwanPositionBottom);
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnNormalEnemy(spwanPositionBottom);
        yield return new WaitForSecondsRealtime(0.5f);
        SpawnNormalEnemy(spwanPositionBottom);
        yield return new WaitForSecondsRealtime(0.5f);


        yield return new WaitForSecondsRealtime(3f);

        //middleのポジションから8体出現
        for (int i = 0; i < 8; i++)
        {
            SpawnNormalEnemy(spawnPositionsMiddle[i]);
            yield return new WaitForSecondsRealtime(0.4f);
        }

        yield return new WaitForSecondsRealtime(3f);

        //middleのポジションから8体出現
        for (int i = 8; i <= 0; i--)
        {
            SpawnNormalEnemy(spawnPositionsMiddle[i]);
            yield return new WaitForSecondsRealtime(0.4f);
        }

        yield return new WaitForSecondsRealtime(3f);

        SpawnNormalEnemy(spawnPositionsMiddle[2]);
        yield return new WaitForSecondsRealtime(0.4f);
        SpawnNormalEnemy(spawnPositionsMiddle[2]);
        yield return new WaitForSecondsRealtime(0.4f);
        SpawnNormalEnemy(spawnPositionsMiddle[2]);
        yield return new WaitForSecondsRealtime(0.4f);
        SpawnNormalEnemy(spawnPositionsMiddle[5]);
        yield return new WaitForSecondsRealtime(0.4f);
        SpawnNormalEnemy(spawnPositionsMiddle[5]);
        yield return new WaitForSecondsRealtime(0.4f);
        SpawnNormalEnemy(spawnPositionsMiddle[5]);
        yield return new WaitForSecondsRealtime(0.4f);

        yield return new WaitForSecondsRealtime(3f);

        SpawnRadialEnemy(radialPositionMiddle);

        yield return new WaitForSecondsRealtime(3f);

        SpawnRadialEnemy(radialPositionTop);
        SpawnRadialEnemy(radialPositionBottom);

        yield return new WaitForSecondsRealtime(4f);


    }


    private void SpawnNormalEnemy(Transform spawnPoint)
    {
        GameObject enemy = Instantiate(nomalEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void SpawnRadialEnemy(Transform spawnPoint)
    {
        GameObject enemy = Instantiate(radialEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}