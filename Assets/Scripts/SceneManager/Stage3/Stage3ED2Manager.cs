using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage3ED2Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeStopPos;
    public Transform pepeEndPos;
    public Image map;
    public GameObject projectionLight;
    public GameObject projectionScreen;
    public Image projectionImage;
    public Sprite[] projectionSprites;

    public Image blackoutPanel;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage3ED2());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage3ED2()
    {
        yield return new WaitForSecondsRealtime(1f);

        player.transform.position = pepeStartPos.position;

        player.transform.DOMove(pepeStopPos.position, 1f);

        yield return new WaitForSeconds(1.5f);

        projectionLight.SetActive(true);
        projectionLight.transform.DOScaleX(0.5f, 0.5f);

        yield return new WaitForSeconds(0.8f);

        projectionScreen.SetActive(true);
        projectionScreen.transform.DOScaleX(0.025f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        projectionImage.sprite = projectionSprites[0];
        projectionImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        projectionImage.sprite = projectionSprites[1];
        projectionImage.gameObject.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        projectionScreen.transform.DOScaleX(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        projectionScreen.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        projectionLight.transform.DOScaleX(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        projectionLight.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        map.gameObject.SetActive(false);

        player.transform.DOShakeScale(1.5f, 50f, 30, 90f, false);
        yield return new WaitForSecondsRealtime(1.5f);

        player.transform.DOMove(pepeEndPos.position, 1f);

        yield return new WaitForSecondsRealtime(2f);

        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);

        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage4OP1");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }

}
