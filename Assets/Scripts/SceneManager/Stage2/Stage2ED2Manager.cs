using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage2ED2Manager : MonoBehaviour
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
            BGMManager.instance.StopBGM();
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage1ED()
    {
        yield return new WaitForSecondsRealtime(1.2f);

        player.transform.localScale = new Vector3(0f, 0f, 0f);
        player.transform.position = pepeStartPos.position;

        player.transform.DOScale(new Vector3(2000f, 2000f, 2000f), 3f);

        player.transform.DOLocalPath(
            new[]{
                pepeStopPos1.position,
                pepeStopPos2.position,
            }, 3f, PathType.CubicBezier);


        yield return new WaitForSecondsRealtime(1f);

        Destroy(player);

        SceneManager.LoadScene("Stage2OP2");
    }
    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }
}
