using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Stage5Manager : MonoBehaviour
{
    public Player3DController player;
    [SerializeField] GameObject god;
    [SerializeField] GameObject godAngryMark;
    [SerializeField] GameObject SpecialGun;
    [SerializeField] Transform godStartPos;
    [SerializeField] Transform godEndPos;
    public TextMeshProUGUI numeratorText;
    public TextMeshProUGUI DenominatorText;
    public TextMeshProUGUI ReleaseText;
    public int SpecialBulletCount = 3;
    public bool isDisplayReleaseText = false;

    public AudioClip stage5BGM;
    public AudioClip stage5LastBGM;

    public UIManager uIManager;

    void Start()
    {
        BGMManager.instance.PlayBGM(stage5BGM);
        uIManager.FadeIn();
        SetNumeratorText(SpecialBulletCount);
        player.EnableShooting();
    }

    private void Update()
    {
        if (player.isSpecialGun)
        {
            if (!isDisplayReleaseText)
            {
                DisplayReleaseText();
                isDisplayReleaseText = true;
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                SpecialBulletCount--;
                SetNumeratorText(SpecialBulletCount);
            }

            if (SpecialBulletCount == 0)
            {
                HideReleaseText();
                player.DisableShooting();
            }
        }
    }

    public IEnumerator StartLastEvent()
    {
        yield return new WaitForSeconds(3f);
        BGMManager.instance.PlayBGM(stage5LastBGM);
        god.SetActive(true);
        god.transform.DOShakePosition(2f, 1f, 100, 90f, false, true);
        god.transform.DOMoveY(godEndPos.position.y, 2f);

        yield return new WaitForSeconds(2f);
        godAngryMark.SetActive(true);
        SpecialGun.SetActive(true);

        yield return new WaitForSeconds(5f);
        god.transform.DOShakePosition(2f, 1f, 100, 90f, false, true);
        god.transform.DOMoveY(godStartPos.position.y, 2f);
        yield return new WaitForSeconds(3f);
        god.SetActive(false);
    }



    public void SetNumeratorText(int num)
    {
        numeratorText.text = num.ToString();
    }

    public void DisplayReleaseText()
    {
        numeratorText.gameObject.SetActive(true);
        DenominatorText.gameObject.SetActive(true);
        ReleaseText.gameObject.SetActive(true);
    }

    public void HideReleaseText()
    {
        numeratorText.gameObject.SetActive(false);
        DenominatorText.gameObject.SetActive(false);
        ReleaseText.gameObject.SetActive(false);
    }
}
