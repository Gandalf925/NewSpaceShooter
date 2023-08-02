using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Stage4Manager : MonoBehaviour
{
    GameObject player;
    PlayerController playerController;
    [Header("Elites")]
    public GameObject magicianPepePrefab;
    public GameObject crownPepePrefab;

    [Header("Boss")]
    public GameObject bossPrefab;
    public Transform bossStartPos;
    public Transform bossStopPos;

    [Header("Normal Enemies")]
    public GameObject[] normalEnemies;
    public Transform NormalStartPos1;
    public Transform NormalStartPos2;
    public Transform NormalStartPos3;

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

        StartCoroutine(StageStart());
    }

    IEnumerator StageStart()
    {
        yield return new WaitForSeconds(6f);
        Debug.Log("Start Stage4");
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
