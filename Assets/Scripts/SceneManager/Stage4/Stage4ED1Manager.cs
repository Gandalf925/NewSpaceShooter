using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Stage4ED1Manager : MonoBehaviour
{
    [SerializeField] Image player;
    [SerializeField] Transform playerStartPos;
    [SerializeField] Transform playerStopPos;
    [SerializeField] Transform playerEndPos;

    [SerializeField] Image bossPepe;
    [SerializeField] Transform bossPepeStartPos;
    [SerializeField] Transform bossPepeEndPos;
    [SerializeField] GameObject bossExplosion;

    [SerializeField] Image subPepe1;
    [SerializeField] Transform subPepe1StartPos;
    [SerializeField] Transform subPepe1EndPos;

    [SerializeField] Image subPepe2;
    [SerializeField] Transform subPepe2StartPos;
    [SerializeField] Transform subPepe2EndPos;
    [SerializeField] Image subPepe3;
    [SerializeField] Transform subPepe3StartPos;
    [SerializeField] Transform subPepe3EndPos;

    public GameObject speachBubble;
    public Image speachImage;
    public Sprite[] speachImageSprites;


    public Image blackoutPanel;

    [SerializeField] AudioClip stage4ED1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        player.transform.position = playerStartPos.position;
        subPepe1.transform.position = subPepe1StartPos.position;
        subPepe2.transform.position = subPepe2StartPos.position;
        subPepe3.transform.position = subPepe3StartPos.position;
        bossPepe.transform.position = bossPepeStartPos.position;
        speachBubble.transform.localScale = new Vector3(0f, 0f, 0f);
        StartCoroutine(Stage4ED1());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage4ED1()
    {
        BGMManager.instance.PlayBGM(stage4ED1BGM);

        subPepe1.transform.DOMove(subPepe1EndPos.position, 80f);
        subPepe1.transform.DORotate(new Vector3(0f, 0f, 360f), 80f, RotateMode.FastBeyond360);
        subPepe2.transform.DOMove(subPepe2EndPos.position, 80f);
        subPepe2.transform.DORotate(new Vector3(0f, 0f, 360f), 80f, RotateMode.FastBeyond360);
        subPepe3.transform.DOMove(subPepe3EndPos.position, 80f);
        subPepe3.transform.DORotate(new Vector3(0f, 0f, 360f), 80f, RotateMode.FastBeyond360);

        bossPepe.transform.DOMove(bossPepeEndPos.position, 4f);
        yield return new WaitForSecondsRealtime(3f);

        player.transform.DOMove(playerStopPos.position, 1f);

        yield return new WaitForSecondsRealtime(1.5f);

        speachImage.sprite = speachImageSprites[0];
        speachBubble.transform.DOScale(new Vector3(1.9f, 1.9f, 1.9f), 2f);

        yield return new WaitForSecondsRealtime(2.5f);

        speachImage.sprite = speachImageSprites[1];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[2];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[3];

        yield return new WaitForSecondsRealtime(3f);

        speachImage.sprite = speachImageSprites[4];

        yield return new WaitForSecondsRealtime(3f);

        speachImage.sprite = speachImageSprites[5];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[6];

        yield return new WaitForSecondsRealtime(3f);

        speachImage.sprite = speachImageSprites[7];

        yield return new WaitForSecondsRealtime(2f);

        speachBubble.transform.DOScale(new Vector3(0f, 0f, 0f), 2f);

        yield return new WaitForSecondsRealtime(1f);

        bossExplosion.SetActive(true);

        yield return new WaitForSecondsRealtime(0.1f);

        bossPepe.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(2f);

        player.transform.DOShakeScale(2f, 0.25f, 100, 40f, false);
        yield return new WaitForSecondsRealtime(2f);
        player.transform.DOShakeScale(3f, 0.5f, 100, 40f, false);
        yield return new WaitForSecondsRealtime(3f);

        player.transform.DOMove(playerEndPos.position, 0.5f);

        yield return new WaitForSecondsRealtime(2f);





        // BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage4ED2");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }

}
