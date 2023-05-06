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

    private void Start()
    {
        blackoutPanel.color = Color.black;
        continueText.color = new Color(255f, 255f, 255f, 0f);
        continueText.gameObject.SetActive(false);
        StartCoroutine(Stage1ED());
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
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[1];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[2];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[3];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[4];
        yield return new WaitForSecondsRealtime(3f);

        bossSpeechBubble.transform.DOScale(new Vector3(0f, 0f, 0f), 2f);

        yield return new WaitForSecondsRealtime(3f);


        GameObject chargeEffect = Instantiate(gunChargePrefab, new Vector3(pepeSpaceGunPos.position.x + 1f, pepeSpaceGunPos.position.y + 0.5f, pepeSpaceGunPos.position.z), Quaternion.identity);
        yield return new WaitForSecondsRealtime(2f);

        bossDialogueImage.sprite = bossSpeeches[5];
        bossSpeechBubble.transform.DOScale(new Vector3(0.03f, 0.015f, 0f), 1f);
        yield return new WaitForSecondsRealtime(3f);

        Destroy(chargeEffect);
        bossDialogueImage.sprite = bossSpeeches[6];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[7];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[8];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[9];
        yield return new WaitForSecondsRealtime(3f);
        bossDialogueImage.sprite = bossSpeeches[10];
        yield return new WaitForSecondsRealtime(3f);

        bossSpeechBubble.transform.DOScale(new Vector3(0, 0, 0f), 1f);
        yield return new WaitForSecondsRealtime(2f);

        defeatedBoss.transform.DOMoveX(bossInitPos.x, 2f);
        yield return new WaitForSecondsRealtime(0.5f);

        Destroy(gun);

        yield return new WaitForSecondsRealtime(1f);
        player.transform.DOShakeScale(2, 60, 20, 100, false);
        yield return new WaitForSecondsRealtime(2f);

        player.transform.DOMove(pepeStopPos.position, 0.3f);

        yield return new WaitForSecondsRealtime(1f);

        blackoutPanel.DOFade(1f, 1f);
        yield return new WaitForSecondsRealtime(2f);
        continueText.gameObject.SetActive(true);

        continueText.DOFade(1f, 3f);

        yield return new WaitForSecondsRealtime(5f);

        continueText.DOFade(0f, 3f);

        yield return new WaitForSecondsRealtime(5f);

        SceneManager.LoadScene("Title");
    }
}
