using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage2OPSecondSM : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeStopPos1;
    public Transform pepeStopPos2;

    public Image blackoutPanel;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage1ED());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage1ED()
    {
        yield return new WaitForSecondsRealtime(1.2f);


        player.transform.position = pepeStartPos.position;
        player.transform.DOMove(pepeStopPos1.position, 2f);


        yield return new WaitForSecondsRealtime(3f);

        player.transform.DOScale(new Vector3(0f, 0f, 0f), 1f);
        player.transform.DOMove(pepeStopPos2.position, 1f);

        yield return new WaitForSecondsRealtime(2f);

        StartCoroutine(SkipScene());
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }
}
