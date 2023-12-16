using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class ED7Manager : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject pepesEgg;
    [SerializeField] GameObject girlsEgg;
    [SerializeField] Transform pepeStartPos;
    [SerializeField] Transform pepeBoundPos;
    [SerializeField] Transform pepeBoundTopPos;
    [SerializeField] Transform pepeBoundStopPos;
    [SerializeField] Transform pepeStopPos1;
    [SerializeField] Transform pepeStopPos2;
    [SerializeField] Transform pepeStopPos3;
    [SerializeField] Transform pepeEndPos;
    [SerializeField] GameObject girl;
    [SerializeField] Transform girlEndPos;
    [SerializeField] Transform girlsEggEndPos;

    [SerializeField] Image girlsSupriseMark;
    [SerializeField] Image speechBubble;
    [SerializeField] Image speechBubbleImage;
    [SerializeField] Sprite[] speechBubbleSprites;

    public Image blackoutPanel;

    [SerializeField] TMP_Text thanksText;
    [SerializeField] TMP_Text cregitsText;
    [SerializeField] Image pepeImage;
    [SerializeField] SpriteRenderer heartSprite;




    [SerializeField] AudioClip ED7BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        player.transform.position = pepeStartPos.position;
        StartCoroutine(ED7());
    }
    private void Update()
    {
    }

    IEnumerator ED7()
    {
        BGMManager.instance.PlayBGM(ED7BGM);
        yield return new WaitForSecondsRealtime(1f);

        yield return new WaitForSecondsRealtime(1f);

        player.transform.DORotate(new Vector3(0, 0, 2000), 1f, RotateMode.FastBeyond360);
        player.transform.DOMove(pepeBoundPos.position, 1f);

        player.transform.DOPath(new Vector3[] { pepeBoundPos.position, pepeBoundTopPos.position, pepeBoundStopPos.position }, 1.5f, PathType.CatmullRom, PathMode.Full3D, 10, null);

        yield return new WaitForSecondsRealtime(0.4f);
        girl.transform.DOPunchPosition(new Vector3(0, 20f, 0), 1f, 1, 1f);
        girlsSupriseMark.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        girlsSupriseMark.gameObject.SetActive(false);
        girl.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSecondsRealtime(2f);

        player.transform.rotation = Quaternion.Euler(0, 0, -30);

        player.transform.DOMove(pepeStopPos1.position, 0.7f);

        yield return new WaitForSecondsRealtime(1f);
        player.transform.DOMove(pepeStopPos2.position, 0.7f);

        yield return new WaitForSecondsRealtime(1f);
        player.transform.DOMove(pepeStopPos3.position, 0.7f);

        yield return new WaitForSecondsRealtime(1f);

        pepesEgg.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);

        pepesEgg.SetActive(false);
        girlsEgg.SetActive(true);

        yield return new WaitForSecondsRealtime(2f);

        girlsEgg.transform.DOMove(girlsEggEndPos.position, 1f);
        girlsEgg.transform.DORotate(new Vector3(0, 0, 360), 1f, RotateMode.FastBeyond360);

        yield return new WaitForSecondsRealtime(0.3f);

        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.transform.DOPunchScale(new Vector3(30f, 30f, 30f), 0.8f, 20, 1f);

        yield return new WaitForSecondsRealtime(2f);

        speechBubble.gameObject.SetActive(true);
        speechBubbleImage.sprite = speechBubbleSprites[0];

        yield return new WaitForSecondsRealtime(2f);

        speechBubbleImage.sprite = speechBubbleSprites[1];

        yield return new WaitForSecondsRealtime(2f);

        speechBubbleImage.sprite = speechBubbleSprites[2];

        yield return new WaitForSecondsRealtime(2f);

        speechBubbleImage.sprite = speechBubbleSprites[3];

        yield return new WaitForSecondsRealtime(2f);

        speechBubble.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(1f);

        girl.transform.rotation = Quaternion.Euler(0, 180, 0);
        girl.transform.DOMoveX(girlEndPos.position.x, 1f);

        yield return new WaitForSecondsRealtime(2f);

        player.transform.DOShakeScale(2f, 100f, 60, 90f, false);
        yield return new WaitForSeconds(2f);
        player.transform.DOMove(pepeEndPos.position, 0.8f);

        yield return new WaitForSecondsRealtime(2f);

        blackoutPanel.DOFade(1f, 1f);

        yield return new WaitForSecondsRealtime(2f);

        pepeImage.DOFade(1f, 1f);
        heartSprite.DOFade(1f, 1f);
        thanksText.DOFade(1f, 1f);
        cregitsText.DOFade(1f, 1f);



    }

}
