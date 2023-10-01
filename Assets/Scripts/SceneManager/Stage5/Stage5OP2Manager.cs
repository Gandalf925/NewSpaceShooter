using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Stage5OP2Manager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject lastBossShadow;
    [SerializeField] GameObject desk;
    [SerializeField] Transform playerEndPos;
    [SerializeField] Transform lastBossEndPos;
    [SerializeField] Transform deskEndPos;


    public Image blackoutPanel;

    [SerializeField] AudioClip stage5OP1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage5OP2());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage5OP2()
    {
        // BGMManager.instance.PlayBGM(stage4OP1BGM);
        yield return new WaitForSecondsRealtime(2f);

        lastBossShadow.GetComponent<Image>().DOFade(1f, 2f);



        yield return new WaitForSecondsRealtime(3f);


        SceneManager.LoadScene("Stage5OP3");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage5");
    }
}
