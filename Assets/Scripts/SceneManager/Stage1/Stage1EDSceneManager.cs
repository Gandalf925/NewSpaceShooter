using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TMPro;

public class Stage1EDSceneManager : MonoBehaviour
{
    public GameObject player;
    public GameObject defeatedBoss;
    public GameObject bossSpeechBubble;
    public Sprite[] bossSpeeches;
    public Image bossDialogueImage;
    public Transform bossStopPos;
    public Transform pepeStopPos;
    public Image blackoutPanel;
    public GameObject pepeSpaceGun;
    public Transform pepeSpaceGunPos;
    public GameObject gunChargePrefab;
    public TMP_Text continueText;

    BGMManager BGMManager;

    public AudioClip Stage1EDBGM;

    private void Start()
    {
        blackoutPanel.color = Color.black;
        continueText.color = new Color(255f, 255f, 255f, 0f);
        continueText.gameObject.SetActive(false);
        BGMManager.instance.PlayBGM(Stage1EDBGM);
        StartCoroutine(Stage1ED());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage1ED()
    {

        Vector3 bossInitPos = new Vector3(defeatedBoss.transform.position.x, transform.position.y, transform.position.z);

        yield return new WaitForSecondsRealtime(1f);

        blackoutPanel.DOFade(0f, 1f);
        yield return new WaitForSecondsRealtime(2f);

        defeatedBoss.transform.DOMoveX(bossStopPos.position.x, 2f);
        yield return new WaitForSecondsRealtime(3f);

        GameObject gun = Instantiate(pepeSpaceGun, pepeSpaceGunPos.position, Quaternion.identity);
        yield return new WaitForSecondsRealtime(1f);

        bossSpeechBubble.transform.DOScale(new Vector3(0.03f, 0.015f, 0f), 2f);

        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[0];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[1];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[2];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[3];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[4];
        yield return new WaitForSecondsRealtime(2f);

        bossSpeechBubble.transform.DOScale(new Vector3(0f, 0f, 0f), 1f);

        yield return new WaitForSecondsRealtime(3f);


        GameObject chargeEffect = Instantiate(gunChargePrefab, new Vector3(pepeSpaceGunPos.position.x + 1f, pepeSpaceGunPos.position.y + 0.5f, pepeSpaceGunPos.position.z), Quaternion.identity);
        StartCoroutine(player.GetComponent<PlayerMovieSE>().PlayChargingSE(5f));
        BGMManager.instance.GetComponent<AudioSource>().volume = 0.2f;

        yield return new WaitForSecondsRealtime(2f);

        bossDialogueImage.sprite = bossSpeeches[5];
        bossSpeechBubble.transform.DOScale(new Vector3(0.03f, 0.015f, 0f), 0.5f);
        yield return new WaitForSecondsRealtime(3f);

        Destroy(chargeEffect);
        BGMManager.instance.GetComponent<AudioSource>().volume = 1f;
        bossDialogueImage.sprite = bossSpeeches[6];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[7];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[8];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[9];
        yield return new WaitForSecondsRealtime(2f);
        bossDialogueImage.sprite = bossSpeeches[10];
        yield return new WaitForSecondsRealtime(2f);

        bossSpeechBubble.transform.DOScale(new Vector3(0, 0, 0f), 1f);
        yield return new WaitForSecondsRealtime(2f);

        defeatedBoss.transform.DOMoveX(bossInitPos.x, 2f);
        yield return new WaitForSecondsRealtime(0.5f);

        Destroy(gun);

        yield return new WaitForSecondsRealtime(1f);
        player.transform.DOShakeScale(2, 0.5f, 20, 100, false);
        yield return new WaitForSecondsRealtime(2f);

        player.transform.DOMove(pepeStopPos.position, 0.3f);

        yield return new WaitForSecondsRealtime(1f);

        SceneManager.LoadScene("Stage2OP1");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage2");
    }
}
