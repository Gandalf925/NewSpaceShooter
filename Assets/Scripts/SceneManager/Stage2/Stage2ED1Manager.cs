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
    public Transform boxMovePath1;
    public Transform boxMovePath2;
    public GameObject egg;
    public GameObject shinyItemEffect;
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
        GameObject pepeBullet1 = Instantiate(bullet, player.transform.position, Quaternion.identity);


        yield return new WaitForSecondsRealtime(0.3f);

        GameObject explosion1 = Instantiate(explosion, explosionPos.transform.position, Quaternion.identity);
        Destroy(pepeBullet1);

        box.transform.DOLocalPath(
            new[]{
            boxMovePath1.position,
            boxMovePath2.position,
        }, 0.5f, PathType.Linear);
        box.transform.DORotate(new Vector3(0f, 0f, 5f), 0.5f);

        yield return new WaitForSeconds(0.3f);

        GameObject shinyEgg = Instantiate(shinyItemEffect, egg.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(1f);

        player.transform.DOShakePosition(2f, 10f, 30, 1, false, false);

        yield return new WaitForSeconds(2.5f);

        player.transform.DOPunchScale(new Vector3(30f, 30f, 0), 1f);
        GameObject pepeBullet2 = Instantiate(bullet, player.transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.3f);

        GameObject explosion2 = Instantiate(explosion, explosionPos.transform.position, Quaternion.identity);
        Destroy(shinyEgg);
        Destroy(pepeBullet2);

        egg.transform.DOLocalPath(
            new[]{
                boxMovePath1.position,
                boxMovePath2.position,
            }, 0.5f, PathType.Linear);

        yield return new WaitForSeconds(1.3f);


        player.transform.DOShakeScale(1.3f, 100f, 60, 90f, false);

        yield return new WaitForSeconds(1.3f);

        player.transform.DOMove(pepeStopPos2.transform.position, 0.3f);

        yield return new WaitForSeconds(1.3f);

        SceneManager.LoadScene("Stage3OP1");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }
}
