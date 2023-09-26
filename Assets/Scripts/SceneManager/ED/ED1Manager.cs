using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ED1Manager : MonoBehaviour
{

    [SerializeField] GameObject bullet;
    [SerializeField] GameObject greenEgg;
    [SerializeField] Transform greenEggEndPos;


    public Image blackoutPanel;

    [SerializeField] AudioClip ED1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(ED1());
    }
    private void Update()
    {
    }

    IEnumerator ED1()
    {
        // BGMManager.instance.PlayBGM(stage5OP1BGM);
        yield return new WaitForSecondsRealtime(1f);

        bullet.SetActive(true);
        bullet.transform.DOMove(greenEgg.transform.position, 0.5f);
        bullet.transform.DOScale(new Vector3(0.5f, 0.5f, 0f), 0.5f);

        yield return new WaitForSecondsRealtime(0.5f);
        bullet.SetActive(false);

        greenEgg.transform.DOMove(greenEggEndPos.position, 0.3f);
        greenEgg.transform.DORotate(new Vector3(0f, 0f, 3000f), 2f);
        greenEgg.transform.DOScale(0.5f, 2f);

        yield return new WaitForSecondsRealtime(1.8f);
        SceneManager.LoadScene("ED2");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }
}
