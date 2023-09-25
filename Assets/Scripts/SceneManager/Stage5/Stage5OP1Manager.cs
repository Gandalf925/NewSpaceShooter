using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class Stage5OP1Manager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject leftDoor;
    [SerializeField] GameObject rightDoor;
    [SerializeField] Transform leftDoorEndPos;
    [SerializeField] Transform rightDoorEndPos;


    public Image blackoutPanel;

    [SerializeField] AudioClip stage5OP1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage5OP1());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage5OP1()
    {
        // BGMManager.instance.PlayBGM(stage4OP1BGM);
        yield return new WaitForSecondsRealtime(2f);

        leftDoor.transform.DOMove(leftDoorEndPos.position, 0.8f);
        leftDoor.transform.DORotate(new Vector3(0f, 0f, 2000f), 0.8f);
        leftDoor.transform.DOScale(new Vector3(10f, 3.2f, 1f), 0.8f);
        rightDoor.transform.DOMove(rightDoorEndPos.position, 0.8f);
        rightDoor.transform.DOScale(new Vector3(10f, 3.8f, 1f), 0.8f);
        rightDoor.transform.DORotate(new Vector3(0f, 0f, -2000f), 0.8f);
        player.transform.DOScale(new Vector3(20f, 20f, 0f), 1.8f);

        yield return new WaitForSecondsRealtime(1.5f);
        SceneManager.LoadScene("Stage5OP2");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage3");
    }
}
