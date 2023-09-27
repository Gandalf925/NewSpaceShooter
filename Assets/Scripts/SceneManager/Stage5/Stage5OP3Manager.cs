using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Stage5OP3Manager : MonoBehaviour
{
    [SerializeField] GameObject lastBossShadow;
    [SerializeField] GameObject greenEgg;
    [SerializeField] Transform greenEggEndPos;
    [SerializeField] GameObject eggEffect;


    public Image blackoutPanel;

    [SerializeField] AudioClip stage5OP1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        eggEffect.transform.localScale = new Vector3(0f, 0f, 0f);
        StartCoroutine(Stage5OP3());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage5OP3()
    {
        yield return new WaitForSecondsRealtime(2f);

        lastBossShadow.GetComponent<Image>().DOColor(new Color(1f, 1f, 1f, 1f), 2f);

        yield return new WaitForSecondsRealtime(2f);

        greenEgg.transform.DOMove(greenEggEndPos.position, 3f);

        yield return new WaitForSecondsRealtime(3f);

        eggEffect.transform.DOScale(new Vector3(7f, 7f, 1f), 3f);

        yield return new WaitForSecondsRealtime(2f);

        lastBossShadow.transform.DOScale(new Vector3(50f, 50f, 0f), 3f);

        yield return new WaitForSecondsRealtime(1.5f);

        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);

        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage5");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage5");
    }
}
