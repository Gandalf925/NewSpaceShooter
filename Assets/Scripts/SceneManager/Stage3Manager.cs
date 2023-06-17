using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Stage3Manager : MonoBehaviour
{
    GameObject player;
    public GameObject enemyPrefab;  // 敵のプレファブ
    public Transform spawnPoint;  // 敵の出現位置

    private int waveCount = 1;  // 現在のウェーブ数
    private int[] enemyCounts = { 1, 1, 2, 1, 2, 2 };  // 各ウェーブの敵の出現数
    private float[] waveDelays = { 5f, 10f, 15f, 10f, 15f, 15f };  // 各ウェーブの開始までの待機時間

    private bool isWaveActive = false;  // ウェーブが進行中かどうか
    private bool isBossAppeared = false;
    public GameObject backgroundPanel;
    [SerializeField] GameObject startTextFrame;
    [SerializeField] Transform frameStartPos;
    [SerializeField] Transform frameStopPos;
    [SerializeField] Transform frameEndPos;
    UIManager uIManager;

    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        uIManager = FindObjectOfType<UIManager>();
        startTextFrame.transform.position = frameStartPos.position;

        uIManager.FadeIn();
        StartCoroutine(StartFrameIn());

        StartCoroutine(StartWaves());
    }

    private void Update()
    {
        if (!isBossAppeared)
        {
            if (waveCount >= enemyCounts.Length && AreAllEnemiesDestroyed())
            {
                isBossAppeared = true;
                Debug.Log("Boss appeared");
            }
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
}