using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage3Manager : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    public GameObject enemyPrefab;  // 敵のプレファブ

    public GameObject warningPanel;
    public Transform playerStayPos;

    public GameObject middleTown;
    public GameObject bossPrefab;
    public Stage3Boss stage3Boss;
    public Transform bossStartPos;

    public GameObject magicCircleToFall;
    public Transform[] magicCirclePos;

    public Transform[] fallObjectsPos;

    public GameObject[] fallObjects;


    private int waveCount = 1;  // 現在のウェーブ数
    private int[] enemyCounts = { 1, 1, 2, 2, 1 };  // 各ウェーブの敵の出現数
    private float[] waveDelays = { 5f, 10f, 15f, 15f, 7f };  // 各ウェーブの開始までの待機時間

    private bool isWaveActive = false;  // ウェーブが進行中かどうか
    private bool isBossAppeared = false;
    public GameObject backgroundPanel;
    [SerializeField] GameObject startTextFrame;
    [SerializeField] Transform frameStartPos;
    [SerializeField] Transform frameStopPos;
    [SerializeField] Transform frameEndPos;
    UIManager uIManager;

    public AudioClip stage3BGM;
    public AudioClip stage3BossBGM;
    private bool hasStartedLoadingNextScene = false;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        playerController = player.GetComponent<PlayerController>();
        uIManager = FindObjectOfType<UIManager>();
        startTextFrame.transform.position = frameStartPos.position;

        uIManager.FadeIn();
        BGMManager.instance.PlayBGM(stage3BGM);
        StartCoroutine(StartFrameIn());

        StartCoroutine(StartWaves());
    }

    private void Update()
    {
        if (!isBossAppeared)
        {
            if (waveCount >= enemyCounts.Length && AreAllEnemiesDestroyed())
            {
                StartCoroutine(BossBattle());
            }
        }
        if (stage3Boss.IsDefeated() && !hasStartedLoadingNextScene)
        {
            hasStartedLoadingNextScene = true;
            StartCoroutine(LoadNextScene());
        }
    }

    IEnumerator StartWaves()
    {
        while (waveCount <= enemyCounts.Length)
        {
            yield return new WaitForSeconds(waveDelays[waveCount - 1]);

            isWaveActive = true;
            StartCoroutine(SpawnEnemies(enemyCounts[waveCount - 1]));
            yield return new WaitUntil(() => isWaveActive == false);

            waveCount++;
        }
    }

    IEnumerator SpawnEnemies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            float x = Random.Range(2f, 8f);
            float y = Random.Range(-4f, 4f);
            Vector3 spawnPosition = new Vector3(x, y, 0f);

            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(1f);  // 敵の出現間隔
        }

        isWaveActive = false;
    }

    bool AreAllEnemiesDestroyed()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject enemy in enemies)
        {
            if (enemy.activeInHierarchy)
            {
                return false; // まだEnemyオブジェクトが存在する
            }
        }

        return true; // すべてのEnemyオブジェクトが破壊された
    }

    IEnumerator StartFrameIn()
    {
        yield return new WaitForSecondsRealtime(1.5f);

        startTextFrame.transform.DOMove(frameStopPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(2f);

        startTextFrame.transform.DOMove(frameEndPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(1f);
    }

    IEnumerator BossBattle()
    {
        isBossAppeared = true;
        StartCoroutine(WarningBeforBossBattle());
        yield return new WaitForSeconds(5f);
        StartCoroutine(StartBossBattle());
    }

    public IEnumerator WarningBeforBossBattle()
    {
        BGMManager.instance.StopBGM();
        playerController.SetPlayerActive(false);
        player.transform.DOMove(new Vector3(playerStayPos.position.x, playerStayPos.position.y, playerStayPos.position.z), 4f);
        StartCoroutine(player.GetComponent<PlayerController>().PlayWarningSE(4f));

        warningPanel.SetActive(true);
        yield return new WaitForSecondsRealtime(4f);
        warningPanel.SetActive(false);

        yield return new WaitForSecondsRealtime(1f);

        BGMManager.instance.bgmSource.volume = 1;
        BGMManager.instance.PlayBGM(stage3BossBGM);

        yield return new WaitForSecondsRealtime(1f);
    }

    IEnumerator StartBossBattle()
    {
        GameObject bossGameObject = Instantiate(bossPrefab, bossStartPos.position, Quaternion.identity);
        stage3Boss = bossGameObject.GetComponent<Stage3Boss>();

        middleTown.transform.DOMoveY(-4.5f, 4f);
        stage3Boss.transform.DOMoveY(-1f, 4f);
        // stage3Boss.transform.DOShakePosition(2f, 10f, 20f, 0, false, false);
        yield return new WaitForSeconds(4f);
        playerController.SetPlayerActive(true);
        // stage3Boss bossController = stage3Boss.GetComponent<Stage3Boss>();
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(2f);

        uIManager.FadeOut();

        yield return new WaitForSeconds(2f);

        BGMManager.instance.StopBGM();
        BGMManager.instance.bgmSource.volume = 0.43f;
        SceneManager.LoadScene("ToBeContinue");
    }
}