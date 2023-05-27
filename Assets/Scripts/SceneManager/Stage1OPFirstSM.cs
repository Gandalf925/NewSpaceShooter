using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;


public class Stage1OPFirstSM : MonoBehaviour
{
    public Transform pepeStopPosition;
    public Transform pepeOutPosition;
    public Transform girlStopPosition;
    public Transform flowerStopPosition;
    public GameObject Flower;
    public GameObject Player;
    public GameObject FrogGirl;
    public GameObject TextArea;
    public GameObject SpeechBubbleImage;
    public Sprite heart;
    public Sprite planet;
    public Sprite greenEgg;
    public Image blackoutPanel;
    public Image textPanel1;

    SoundManager soundManager;

    public AudioClip OpBGM1;
    public AudioClip OpBGM2;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<AudioSource>().GetComponent<SoundManager>();
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            soundManager.StopBGM();
            StartCoroutine(SkipScene());
        }
    }

    public IEnumerator StartScene()
    {
        // soundManager.PlayBGM(OpBGM1);
        yield return new WaitForSeconds(1f);
        Player.transform.DOMoveX(pepeStopPosition.position.x, 1f);
        yield return new WaitForSeconds(1f);
        TextArea.transform.DOScale(new Vector3(9f, 4f, 0), 1f);
        yield return new WaitForSeconds(2f);
        SpeechBubbleImage.GetComponent<Image>().sprite = planet;
        yield return new WaitForSeconds(2f);
        SpeechBubbleImage.GetComponent<Image>().sprite = greenEgg;
        yield return new WaitForSeconds(2f);
        SpeechBubbleImage.GetComponent<Image>().sprite = heart;
        yield return new WaitForSeconds(2f);
        TextArea.transform.DOScale(new Vector3(0, 0, 0), 1f);
        soundManager.StopBGM();


        yield return new WaitForSeconds(1f);
        // soundManager.PlayBGM(OpBGM2);
        FrogGirl.transform.DOScaleX(-4.94f, 0.01f);
        FrogGirl.transform.DOMoveX(girlStopPosition.position.x, 2f);


        yield return new WaitForSeconds(2f);
        Player.transform.DOShakePosition(2f, 10f, 30, 1, false, false);

        yield return new WaitForSeconds(1.5f);
        Flower.transform.DORotate(new Vector3(0, 0, 5000), 1, RotateMode.FastBeyond360);
        Flower.transform.DOMove(flowerStopPosition.transform.position, 1f);
        yield return new WaitForSeconds(1.5f);
        Player.transform.DOShakeScale(1f, 100f, 60, 90f, false);
        yield return new WaitForSeconds(1f);
        Player.transform.DOMove(pepeOutPosition.transform.position, 0.8f);

        yield return new WaitForSeconds(1f);
        blackoutPanel.DOFade(1f, 2f);

        LoadNextScene();
    }

    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stage1");
    }
    private void LoadNextScene()
    {
        SceneManager.LoadScene("Stage1OPSecond");
    }
}
