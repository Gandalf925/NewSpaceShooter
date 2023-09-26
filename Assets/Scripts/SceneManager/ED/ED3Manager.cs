using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ED3Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeTurnPos;
    public Transform pepeStopPos;
    public Transform front1;
    public Transform front2;
    public Transform front3;
    public Transform back1;
    public Transform back2;
    public Transform CEO;
    public Transform bartender;
    public Transform randy;
    public Transform randyStartPos;
    public Transform randyEndPos;


    public Image blackoutPanel;

    [SerializeField] AudioClip ED3BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        player.transform.position = pepeStartPos.position;
        randy.transform.position = new Vector3(0f, -100f, 0f);
        StartCoroutine(ED3());
    }
    private void Update()
    {

    }

    IEnumerator ED3()
    {
        // BGMManager.instance.PlayBGM(stage5OP1BGM);
        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(cheerPepes(front1));
        StartCoroutine(cheerPepes(front2));
        StartCoroutine(cheerPepes(front3));
        StartCoroutine(cheerPepes(back1));
        StartCoroutine(cheerPepes(back2));
        StartCoroutine(cheerPepes(CEO));
        StartCoroutine(cheerPepes(bartender));

        yield return new WaitForSecondsRealtime(1f);

        randy.transform.DOMove(randyStartPos.position, 1f);

        randy.transform.DOMove(randyEndPos.position, 2f);

        player.transform.DOPath(new Vector3[] { pepeStartPos.position, pepeTurnPos.position, pepeStopPos.position }, 5f, PathType.CatmullRom, PathMode.Full3D, 10, null).SetEase(Ease.Linear);

        player.transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)  // 線形のイージングを使用して一定速度で回転
            .SetLoops(-1, LoopType.Incremental);  // 無限にループさせる

        yield return new WaitForSecondsRealtime(2f);
        randy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        yield return new WaitForSecondsRealtime(1f);

        randy.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        yield return new WaitForSecondsRealtime(1f);

        randy.transform.rotation = Quaternion.Euler(0f, 180f, 0f);

        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("ED4");
    }


    IEnumerator cheerPepes(Transform pepe)
    {
        float randomNumber = Random.Range(0, 0.2f);
        yield return new WaitForSeconds(randomNumber);

        for (int i = 0; i <= 50; i++)
        {
            pepe.DOPunchPosition(pepe.position + new Vector3(0f, 10f, 0f), 1, 1);

            yield return new WaitForSeconds(1f);

            yield return new WaitForSeconds(randomNumber);
        }

    }

}
