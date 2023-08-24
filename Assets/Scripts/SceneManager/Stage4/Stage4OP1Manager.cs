using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage4OP1Manager : MonoBehaviour
{
    [SerializeField] Image POPanel;

    public Image blackoutPanel;

    private void Start()
    {

        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage4OP1());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage4OP1()
    {
        yield return new WaitForSecondsRealtime(2f);

        POPanel.gameObject.transform.DOScale(2f, 1f);

        yield return new WaitForSecondsRealtime(1f);

        POPanel.DOFade(0f, 1f);

        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("Stage4OP2");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }

}
