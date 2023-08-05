using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stage4Manager : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;

    public Transform canvasTransform;

    [Header("Elites")]
    public GameObject magicianPepePrefab;
    public GameObject crownPepePrefab;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossStartPos;
    public Transform bossStopPos;

    [Header("Normal Enemies")]
    public GameObject[] normalEnemies;
    public Transform[] normalStartPos;

    [Header("StartTextFrame")]
    [SerializeField] GameObject startTextFrame;
    [SerializeField] Transform frameStartPos;
    [SerializeField] Transform frameStopPos;
    [SerializeField] Transform frameEndPos;

    [Header("Manager")]
    GameManager gameManager;
    UIManager uIManager;

    public AudioClip stage4BGM;
    public AudioClip stage4BossBGM;


    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
        playerController = player.GetComponent<PlayerController>();
        uIManager = FindObjectOfType<UIManager>();
        startTextFrame.transform.position = frameStartPos.position;

        uIManager.FadeIn();
        // BGMManager.instance.PlayBGM(stage3BGM);
        StartCoroutine(StartFrameIn());

        StartCoroutine(SpawnEnemies(6f));
    }

    private IEnumerator SpawnEnemies(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);

        // 6秒後に強敵を生成
        GameObject strongEnemy1 = Instantiate(magicianPepePrefab, canvasTransform);

        while (strongEnemy1 != null && strongEnemy1.activeSelf)
        {
            yield return new WaitForSeconds(4f);

            Transform spawnPos = normalStartPos[Random.Range(0, normalStartPos.Length)];
            GameObject normalEnemy = normalEnemies[Random.Range(0, normalEnemies.Length)];
            Instantiate(normalEnemy, spawnPos.position, Quaternion.identity, canvasTransform);
        }

        // 強敵の死亡を監視
        while (strongEnemy1 != null && strongEnemy1.activeSelf)
        {
            yield return null;
        }

        // 強敵が倒されたら、3秒後に次の強敵を生成
        yield return new WaitForSeconds(3f);

        GameObject strongEnemy2 = Instantiate(crownPepePrefab, canvasTransform);

        while (strongEnemy2 != null && strongEnemy2.activeSelf)
        {
            yield return new WaitForSeconds(4f);

            Transform spawnPos = normalStartPos[Random.Range(0, normalStartPos.Length)];
            GameObject normalEnemy = normalEnemies[Random.Range(0, normalEnemies.Length)];
            Instantiate(normalEnemy, spawnPos.position, Quaternion.identity, canvasTransform);
        }


        // 強敵の死亡を監視
        while (strongEnemy2 != null && strongEnemy2.activeSelf)
        {
            yield return null;
        }

        yield return new WaitForSeconds(3f);



        Instantiate(bossPrefab, canvasTransform);
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
