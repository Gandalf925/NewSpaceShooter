using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject[] lifeObjects;
    public Image pauseButtonIcon;
    public Sprite pauseImage;
    public Sprite playbackImage;

    public Button fullscreenButton;
    public Image fullscreenButtonIcon;
    public Sprite fullscreenIcon;
    public Sprite windowIcon;


    public Image blackoutPanel;

    public void FadeIn()
    {
        blackoutPanel.DOFade(0f, 2f);
    }
    public void FadeOut()
    {
        blackoutPanel.DOFade(1f, 2f);
    }
}
