using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage4OP1Manager : MonoBehaviour
{
    [SerializeField] Image POPanel;
    [SerializeField] Image PMPanel;
    [SerializeField] Image SPepeMoviePanel;
    [SerializeField] Image bossPepe;
    [SerializeField] Sprite bossPepeBlue;
    [SerializeField] Transform bossPepeEndPos;
    [SerializeField] Transform greenEgg;
    [SerializeField] Transform greenEggEndPos;
    [SerializeField] ParticleSystem eggParticle;
    [SerializeField] Transform upperGreenEgg;
    [SerializeField] Transform upperEggEndPos;


    public Image blackoutPanel;

    private void Start()
    {

        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        eggParticle.transform.localScale = new Vector3(0f, 0f, 0f);
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

        yield return new WaitForSecondsRealtime(2.5f);

        PMPanel.DOFade(0f, 1f);
        bossPepe.transform.DOShakeScale(7f, 0.5f, 40, 90f, false);

        yield return new WaitForSecondsRealtime(2.5f);

        eggParticle.transform.DOScale(new Vector3(20f, 20f, 2f), 2f);

        yield return new WaitForSecondsRealtime(3f);

        bossPepe.GetComponent<Image>().sprite = bossPepeBlue;

        yield return new WaitForSecondsRealtime(1.5f);

        eggParticle.transform.DOScale(new Vector3(0f, 0f, 0f), 2f);

        yield return new WaitForSecondsRealtime(0.5f);

        bossPepe.transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        yield return new WaitForSecondsRealtime(1.2f);

        bossPepe.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        yield return new WaitForSecondsRealtime(0.7f);
        bossPepe.transform.DOMoveX(bossPepeEndPos.position.x, 2f);

        yield return new WaitForSecondsRealtime(1.3f);

        greenEgg.transform.DOMoveY(greenEggEndPos.position.y, 2f);

        yield return new WaitForSecondsRealtime(2f);

        SPepeMoviePanel.DOFade(0f, 1f);

        yield return new WaitForSecondsRealtime(2f);

        upperGreenEgg.transform.DOMoveY(upperEggEndPos.position.y, 2f);

        yield return new WaitForSecondsRealtime(2f);

        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2.5f);


        SceneManager.LoadScene("Stage4");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }

}
