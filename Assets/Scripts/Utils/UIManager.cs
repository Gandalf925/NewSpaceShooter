using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    public GameObject[] lifeObjects;

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
