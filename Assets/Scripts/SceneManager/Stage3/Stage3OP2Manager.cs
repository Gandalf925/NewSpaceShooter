using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage3OP2Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStopPos;
    public Transform bartender;
    public GameObject sweat;
    public GameObject cocktail;
    public Transform cocktailStartPos;
    public Transform cocktailStopPos;
    public GameObject speachBubble;
    public Image speachImage;
    public Sprite[] speachImageSprites;
    public GameObject background0;
    public GameObject background1;

    public Image blackoutPanel;

    SEManager SEManager;
    public AudioClip Stage2EDBGM2;
    public AudioClip Stage2EDPutTheGlassOnSE;
    public AudioClip Stage2EDSlideTheGlassSE;
    public AudioClip Stage2EDQuakeSE;



    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 255f);
        GameObject SEManagerObj = GameObject.FindGameObjectWithTag("SEManager");
        SEManager = SEManagerObj.GetComponent<SEManager>();
        BGMManager.instance.PlayBGM(Stage2EDBGM2);
        StartCoroutine(Stage3OP2());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage3OP2()
    {
        blackoutPanel.DOFade(0, 2f);
        yield return new WaitForSecondsRealtime(3f);
        cocktail.transform.position = cocktailStartPos.position;
        cocktail.SetActive(true);
        SEManager.PlaySE(Stage2EDPutTheGlassOnSE);

        yield return new WaitForSecondsRealtime(1f);
        cocktail.transform.DOMove(cocktailStopPos.position, 1.5f);
        SEManager.PlaySE(Stage2EDSlideTheGlassSE);


        yield return new WaitForSecondsRealtime(2f);

        speachBubble.transform.localScale = new Vector3(0f, 0f, 0f);
        speachImage.sprite = speachImageSprites[0];
        speachBubble.SetActive(true);
        speachBubble.transform.DOScale(new Vector3(4.35f, 4.35f, 4.35f), 1f);

        yield return new WaitForSecondsRealtime(1.8f);

        player.transform.DOShakePosition(1f, 10f, 30, 1, false, false);

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[1];

        yield return new WaitForSecondsRealtime(2f);

        player.transform.DOShakePosition(1f, 10f, 30, 1, false, false);

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[2];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[3];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[4];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[5];

        yield return new WaitForSecondsRealtime(2f);

        player.transform.DOShakePosition(1f, 50f, 30, 1, false, false);

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[6];

        yield return new WaitForSecondsRealtime(2f);

        SEManager.PlaySE(Stage2EDQuakeSE);

        player.transform.DOShakePosition(2f, 20f, 30, 1, false, false);
        background0.transform.DOShakePosition(2f, 20f, 30, 1, false, false);
        background1.transform.DOShakePosition(2f, 20f, 30, 1, false, false);
        bartender.transform.DOShakePosition(2f, 20f, 30, 1, false, false);
        cocktail.transform.DOShakePosition(2f, 20f, 30, 1, false, false);

        yield return new WaitForSecondsRealtime(2.5f);
        SEManager.StopSE();

        sweat.SetActive(true);
        sweat.transform.DOMoveY(-0.05f, 1f);


        yield return new WaitForSecondsRealtime(2f);

        sweat.SetActive(false);

        speachImage.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        speachImage.sprite = speachImageSprites[7];
        yield return new WaitForSecondsRealtime(2f);

        speachImage.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
        speachImage.sprite = speachImageSprites[8];

        yield return new WaitForSecondsRealtime(2f);

        speachImage.sprite = speachImageSprites[9];

        yield return new WaitForSecondsRealtime(2f);

        player.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSecondsRealtime(1f);

        player.transform.DOMoveX(pepeStopPos.position.x, 3f);

        yield return new WaitForSecondsRealtime(2f);

        blackoutPanel.DOFade(1f, 2f);

        yield return new WaitForSecondsRealtime(2f);

        BGMManager.instance.StopBGM();

        SceneManager.LoadScene("Stage3");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }
}
