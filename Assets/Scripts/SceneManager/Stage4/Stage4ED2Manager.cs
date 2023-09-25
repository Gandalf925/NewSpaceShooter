using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class Stage4ED2Manager : MonoBehaviour
{
    [SerializeField] Image player;
    [SerializeField] Transform playerStartPos;
    [SerializeField] Transform playerStopPos1;
    [SerializeField] Transform playerStopPos2;
    [SerializeField] Transform playerStopPos3;
    [SerializeField] Transform playerEndPos;

    [SerializeField] GameObject POPanel;
    public Image blackoutPanel;

    [SerializeField] AudioClip stage4OP1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        player.transform.position = playerStartPos.position;
        player.transform.localScale = new Vector3(3f, 3f, 3f);
        StartCoroutine(Stage4ED2());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage4ED2()
    {
        // BGMManager.instance.PlayBGM(stage4OP1BGM);
        yield return new WaitForSeconds(1f);

        player.transform.DOScale(new Vector3(0f, 0f, 0f), 2f);

        player.transform.DOMove(playerStopPos1.position, 0.5f);

        yield return new WaitForSeconds(0.5f);


        player.transform.DOMove(playerStopPos2.position, 0.5f);

        yield return new WaitForSeconds(0.5f);


        player.transform.DOMove(playerStopPos3.position, 0.5f);

        yield return new WaitForSeconds(0.5f);


        player.transform.DOMove(playerEndPos.position, 0.5f);

        yield return new WaitForSeconds(0.5f);

        POPanel.transform.DOScale(new Vector3(3f, 3f, 3f), 0.5f);

        yield return new WaitForSeconds(0.5f);

        blackoutPanel.DOFade(1f, 2f);

        yield return new WaitForSeconds(2f);

        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage5OP1");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }

}
