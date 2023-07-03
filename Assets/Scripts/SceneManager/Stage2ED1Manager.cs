using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Stage2ED1Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeStopPos1;
    public Transform pepeStopPos2;
    public GameObject box;
    public GameObject egg;
    public GameObject explosion;
    public Transform explosionPos;
    public GameObject bullet;

    public Image blackoutPanel;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage2ED());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage2ED()
    {
        yield return new WaitForSecondsRealtime(1.2f);


        player.transform.position = pepeStartPos.position;
        player.transform.DOMove(pepeStopPos1.position, 2f);


        yield return new WaitForSecondsRealtime(2.5f);

        player.transform.DOPunchScale(new Vector3(30f, 30f, 0), 1f);
        bullet = Instantiate(bullet, player.transform.position, Quaternion.identity);


        yield return new WaitForSecondsRealtime(0.5f);

        explosion = Instantiate(explosion, explosionPos.transform.position, Quaternion.identity);

        // box.transform.DOPath()

        // StartCoroutine(SkipScene());
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }
}
