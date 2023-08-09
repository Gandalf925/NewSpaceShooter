using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stage4Manager : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    public Transform playerStayPos;

    public Transform canvasTransform;

    public GameObject warningPanel;

    [Header("Elites")]
    public GameObject magicianPepePrefab;
    public GameObject crownPepePrefab;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossStayPos1;
    public Transform bossStayPos2;
    public GameObject[] shieldPepes;
    public Transform[] shieldPepePos;
    public Transform[] shieldPepeStartPos;

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

        yield return new WaitForSeconds(2f);

        StartCoroutine(WarningBeforBossBattle());
        yield return new WaitForSeconds(7f);


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
        // BGMManager.instance.PlayBGM(stage3BossBGM);

        yield return new WaitForSecondsRealtime(3f);
        playerController.SetPlayerActive(true);
    }

    public IEnumerator SetShieldPepes()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < shieldPepes.Length; i++)
        {
            GameObject shieldPepe = Instantiate(shieldPepes[i], canvasTransform);
            shieldPepe.transform.position = shieldPepeStartPos[i].position;
            shieldPepe.transform.DOMove(shieldPepePos[i].transform.position, 1f);
        }
        yield return new WaitForSeconds(2f);
    }
}
