using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class ToBeContinueManager : MonoBehaviour
{
    public Image blackoutPanel;

    void Start()
    {
        StartCoroutine(Blackout());
    }

    IEnumerator Blackout()
    {
        yield return new WaitForSeconds(1f);

        blackoutPanel.DOFade(0f, 3f);

        yield return new WaitForSeconds(6f);

        blackoutPanel.DOFade(1f, 3f);

        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("Title");
    }
}
