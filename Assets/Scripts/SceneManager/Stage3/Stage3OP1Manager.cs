using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage3OP1Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeStopPos;
    public Transform front1;
    public Transform front2;
    public Transform front3;
    public Transform back1;
    public Transform back2;
    public Transform CEO;
    public Transform walker;
    public Transform walkerStartPos;
    public Transform walkerStopPos;
    public Transform frontPepeStopPos;
    public Transform backPepeStopPos;
    public GameObject backgroundPanel;


    public SpriteRenderer blackoutPanel;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage3OP1());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage3OP1()
    {
        yield return new WaitForSecondsRealtime(1f);
        walker.position = walkerStartPos.position;
        walker.DOMoveX(walkerStopPos.position.x, 6f);

        StartCoroutine(SpeakerMove(front1));
        StartCoroutine(SpeakerMove(front2));
        StartCoroutine(SpeakerMove(front3));
        StartCoroutine(SpeakerMove(back1));
        StartCoroutine(SpeakerMove(back2));

        yield return new WaitForSecondsRealtime(4f);

        PepeFadeOut(front1);
        PepeFadeOut(front2);
        PepeFadeOut(front3);

        yield return new WaitForSecondsRealtime(1f);

        PepeFadeOut(back1);
        PepeFadeOut(back2);



        player.transform.position = pepeStartPos.position;
        player.transform.DOMove(pepeStopPos.position, 2f);
        yield return new WaitForSeconds(1.5f);
        player.transform.DOScale(0f, 0.5f);
        yield return new WaitForSeconds(0.5f);
        Destroy(player);


        yield return new WaitForSecondsRealtime(1f);

        backgroundPanel.transform.DOScale(new Vector3(200f, 150f, 100f), 1f);

        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene("Stage3OP2");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }

    IEnumerator SpeakerMove(Transform pepe)
    {
        float randomNumber = Random.Range(0, 0.4f);
        yield return new WaitForSeconds(randomNumber);

        for (int i = 0; i <= 10; i++)
        {
            pepe.DOPunchPosition(pepe.position + new Vector3(0f, 10f, 0f), 1, 1);

            yield return new WaitForSeconds(1f);

            yield return new WaitForSeconds(randomNumber);
        }

    }

    void PepeFadeOut(Transform pepe)
    {
        // pepe.DOMoveX(frontPepeStopPos.position.x, 2f);
        pepe.GetComponent<SpriteRenderer>().DOFade(0f, 2);
        pepe.DOScale(100f, 2f);
    }
}
