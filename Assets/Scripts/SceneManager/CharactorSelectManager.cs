using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class CharactorSelectManager : MonoBehaviour
{
    PlayerImageManager playerImageManager;  // PlayerImageManagerコンポーネント
    GetCharacterSprite getCharacterSprite;  // GetCharacterSpriteコンポーネント
    public List<Sprite> characters = new List<Sprite>();  // キャラクターのスプライトリスト
    public Image characterDisplay;   // キャラクターを表示するImageコンポーネント
    private int currentIndex = 0;    // 現在選択されているキャラクターのインデックス
    [SerializeField] GameObject blackPanel;
    public GameObject enterAddressPopup;
    public GameObject enterAddressPanel;

    public TMP_Text enterAddressInputField;

    void Start()
    {
        FadeOut();
        // 最初のキャラクターを表示
        if (characters.Count > 0)
        {
            characterDisplay.sprite = characters[currentIndex];
        }
    }

    // 次のキャラクターを表示するメソッド
    public void NextCharacter()
    {
        if (characters.Count > 0)
        {
            currentIndex = (currentIndex + 1) % characters.Count;
            characterDisplay.sprite = characters[currentIndex];
        }
    }

    // 前のキャラクターを表示するメソッド
    public void PreviousCharacter()
    {
        if (characters.Count > 0)
        {
            currentIndex = (currentIndex - 1 + characters.Count) % characters.Count;
            characterDisplay.sprite = characters[currentIndex];
        }
    }

    public void GetCharacterSpriteList()
    {
        characters = PlayerImageManager.Instance.images;
    }

    public void FadeOut()
    {
        Image image = blackPanel.GetComponent<Image>();
        image.DOFade(0f, 1f).OnComplete(() => blackPanel.SetActive(false));
    }

    public void FadeIn()
    {
        blackPanel.SetActive(true);
        Image image = blackPanel.GetComponent<Image>();
        image.DOFade(1f, 1f);
    }

    public void PushLaunchButton()
    {
        StartCoroutine(NextScene());
    }

    private IEnumerator NextScene()
    {
        PlayerImageManager.Instance.selectedCharacter = characters[currentIndex];

        FadeIn();

        yield return new WaitForSeconds(1.2f);
        SceneManager.LoadScene("Stage1OPFirst");
    }


}