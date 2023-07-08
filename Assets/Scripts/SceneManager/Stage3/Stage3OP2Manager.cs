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
    public GameObject cocktail;
    public Transform cocktailStartPos;
    public Transform cocktailStopPos;
    public GameObject speachBubble;
    public Image speachImage;
    public Sprite[] speachImageSprites;
    public GameObject backgroundPanel;

    public Image blackoutPanel;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 255f);
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
        yield return new WaitForSecondsRealtime(1f);
        cocktail.transform.DOMove(cocktailStopPos.position, 1.5f);

        yield return new WaitForSecondsRealtime(2f);

        speachBubble.transform.localScale = new Vector3(0f, 0f, 0f);
        speachBubble.SetActive(true);
        speachBubble.transform.DOScale(new Vector3(4.35f, 4.35f, 4.35f), 1f);


        yield return new WaitForSecondsRealtime(5f);


        SceneManager.LoadScene("Stage3");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }
}
