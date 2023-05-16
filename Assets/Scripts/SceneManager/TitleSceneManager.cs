using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class TitleSceneManager : MonoBehaviour
{
    private string sceneName = "Stage1OPFirst";
    public Image titleTextImage;
    public Image pressSpaceKeyImage;

    public GameObject blackoutPanel;

    public Transform titleTextStopPosition;
    public Transform pressSpaceImageStopPosition;
    SoundManager soundManager;
    public AudioClip titleBGM;
    public AudioClip tapSE;

    bool isStart = false;

    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<AudioSource>().GetComponent<SoundManager>();
        soundManager.PlayBGM(titleBGM);
        StartCoroutine(MoveTitleText());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStart)
        {
            if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            {
                soundManager.PlaySE(tapSE);
                isStart = true;
                StartCoroutine(LoadNextScene());
            }
        }
    }

    IEnumerator MoveTitleText()
    {
        yield return new WaitForSecondsRealtime(2f);

        blackoutPanel.GetComponent<Image>().DOFade(0f, 1f);

        yield return new WaitForSecondsRealtime(1f);

        titleTextImage.transform.DOMoveY(titleTextStopPosition.position.y, 3f);

        yield return new WaitForSecondsRealtime(3f);

        pressSpaceKeyImage.transform.DOMoveY(pressSpaceImageStopPosition.position.y, 1f);
    }
    IEnumerator LoadNextScene()
    {
        pressSpaceKeyImage.DOFade(0f, 0.2f);
        yield return new WaitForSecondsRealtime(0.2f);
        pressSpaceKeyImage.DOFade(1f, 0.2f);
        yield return new WaitForSecondsRealtime(0.2f);
        pressSpaceKeyImage.DOFade(0f, 0.2f);
        yield return new WaitForSecondsRealtime(0.2f);
        pressSpaceKeyImage.DOFade(1f, 0.2f);
        yield return new WaitForSecondsRealtime(0.2f);

        blackoutPanel.GetComponent<Image>().DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(1f);
        SceneManager.LoadScene(sceneName);
    }
}
