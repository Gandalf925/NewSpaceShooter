using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class EnemySpawnerStage1 : MonoBehaviour
{
    public GameObject nomalEnemyPrefab;
    public GameObject radialEnemyPrefab;
    public GameObject bossPrefab;
    Stage1BossController boss;

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
    public PlayerController player;
    private bool allEnemiesSpawned = false;
    public GameObject warningPanel;
    BackgroundController backgroundController;
    BackgroundPanelShrink backgroundShrinker;
    public GameObject bossDecoy;

    [SerializeField] GameObject startTextFrame;
    [SerializeField] Transform frameStartPos;
    [SerializeField] Transform frameStopPos;
    [SerializeField] Transform frameEndPos;

    bool bossAppear = false;
    bool atOnce = false;

    private Vector3 playerInitialScale;
    [SerializeField] Image blackoutPanel;
    [SerializeField] GameObject backgroundStarsPanel;
    private string nextSceneName = "Stage1ED";

    void Start()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnBossRoutine());
        startTextFrame.transform.position = frameStartPos.position;
        backgroundController = FindObjectOfType<BackgroundController>();
        backgroundShrinker = backgroundPanel.GetComponent<BackgroundPanelShrink>();
        player = FindObjectOfType<PlayerController>();
        blackoutPanel.color = new Color(0f, 0f, 0f, 255f);
    }

    void Update()
    {
        if (bossAppear)
        {
            if (boss.isDefeated && !atOnce)
            {
                atOnce = true;
                StartCoroutine(LoadNextScene());
            }
        }
    }

    IEnumerator SpawnRoutine()
    {
        blackoutPanel.DOFade(0f, 3f);
        // Game Start
        yield return new WaitForSecondsRealtime(5f);

        startTextFrame.transform.DOMove(frameStopPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(2f);

        startTextFrame.transform.DOMove(frameEndPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(1f);

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
        boss = Instantiate(bossPrefab, spawnPoint.position, Quaternion.identity).GetComponent<Stage1BossController>();
        bossAppear = true;
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

    IEnumerator LoadNextScene()
    {
        EnemyBulletController[] enemyBullets = FindObjectsOfType<EnemyBulletController>();
        for (int i = 0; i < enemyBullets.Length; i++)
        {
            enemyBullets[i].Destroy();
        }

        BossBeamController bossBeam = FindObjectOfType<BossBeamController>();
        bossBeam.Destroy();

        yield return new WaitForSecondsRealtime(1f);
        blackoutPanel.DOFade(1f, 2f);
        backgroundStarsPanel.SetActive(false);
        yield return new WaitForSecondsRealtime(2f);
        SceneManager.LoadScene(nextSceneName);
    }
}