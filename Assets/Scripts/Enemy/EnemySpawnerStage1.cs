using System.Collections;
using UnityEngine;
using DG.Tweening;

public class EnemySpawnerStage1 : MonoBehaviour
{
    public GameObject nomalEnemyPrefab;
    public GameObject radialEnemyPrefab;
    public GameObject bossPrefab;

    [Header("NormalEnemyPosition")]
    public Transform spawnPositionTop;
    public Transform[] spawnPositionsMiddle;
    public Transform spwanPositionBottom;

    [Header("RadialEnemyPosition")]
    public Transform radialPositionTop;
    public Transform radialPositionMiddle;
    public Transform radialPositionBottom;

    [Header("Boss")]
    public Transform bossSpawnPosition;
    public GameObject backgroundPanel;
    public GameObject player;
    private bool allEnemiesSpawned = false;
    public GameObject warningPanel;
    BackgroundController backgroundController;
    BackgroundPanelShrink backgroundShrinker;
    public GameObject bossDecoy;

    private Vector3 playerInitialScale;
    public float scaleDuration = 0.8f;
    public float scaleAmount = 0.8f;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnBossRoutine());
        backgroundController = FindObjectOfType<BackgroundController>();
        backgroundShrinker = backgroundPanel.GetComponent<BackgroundPanelShrink>();
        player = GameObject.FindWithTag("Player");

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

        allEnemiesSpawned = true;

    }

    IEnumerator SpawnBossRoutine()
    {
        // すべての敵が生成されるまで待つ
        while (!allEnemiesSpawned)
        {
            yield return null;
        }

        while (!AreAllEnemiesDefeated())
        {
            yield return null;
        }

        yield return new WaitForSeconds(3f);


        StartCoroutine(BossAppearanceDirection());


        yield return new WaitForSecondsRealtime(8f);

        bossDecoy.SetActive(false);
        // Spawn the boss
        SpawnBoss(bossSpawnPosition);
    }


    private void SpawnNormalEnemy(Transform spawnPoint)
    {
        GameObject enemy = Instantiate(nomalEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }

    private void SpawnRadialEnemy(Transform spawnPoint)
    {
        GameObject enemy = Instantiate(radialEnemyPrefab, spawnPoint.position, Quaternion.identity);
    }
    private void SpawnBoss(Transform spawnPoint)
    {
        GameObject boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity);
    }

    private bool AreAllEnemiesDefeated()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length == 0;
    }

    private IEnumerator BossAppearanceDirection()
    {
        StartCoroutine(StopBackgroundMove());
        warningPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        warningPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(1f);
        backgroundShrinker.Shrink();
        yield return new WaitForSecondsRealtime(3f);

    }

    private IEnumerator StopBackgroundMove()
    {
        for (int i = 0; i <= 4; i++)
        {
            backgroundController.scrollSpeedFront -= 2;
            backgroundController.scrollSpeedBack -= 1;
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}