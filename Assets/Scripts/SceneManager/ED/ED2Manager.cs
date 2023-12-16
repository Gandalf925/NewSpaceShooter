using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ED2Manager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject greenEgg;
    [SerializeField] GameObject sun;
    [SerializeField] Transform playerStopPos;
    [SerializeField] Transform playerMovePos1;
    [SerializeField] Transform playerMovePos2;
    [SerializeField] Transform playerMovePos3;
    [SerializeField] Transform playerEndPos;
    [SerializeField] Transform greenEggStopPos;
    [SerializeField] Transform greenEggEndPos;


    public Image blackoutPanel;

    [SerializeField] AudioClip ED1BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(ED1());
    }
    private void Update()
    {

    }

    IEnumerator ED1()
    {
        // BGMManager.instance.PlayBGM(stage5OP1BGM);
        yield return new WaitForSecondsRealtime(1f);

        sun.transform.DOScale(200f, 12f);

        greenEgg.SetActive(true);
        greenEgg.transform.DOScale(30f, 5f);
        greenEgg.transform.DORotate(new Vector3(0f, 0f, 2000f), 11f, RotateMode.FastBeyond360);
        greenEgg.transform.DOMove(greenEggStopPos.position, 3f);


        yield return new WaitForSecondsRealtime(2f);
        player.SetActive(true);
        player.transform.DOShakeScale(2f, 1f, 100, 90f, false);
        player.transform.DOMove(playerStopPos.position, 6f);
        player.transform.DOScale(15f, 3f);

        yield return new WaitForSecondsRealtime(2f);
        player.transform.DOShakeScale(2f, 6f, 100, 90f, false);

        yield return new WaitForSecondsRealtime(2f);
        player.transform.DOShakeScale(2f, 15f, 100, 90f, false);
        yield return new WaitForSecondsRealtime(2f);
        player.transform.DOMove(playerMovePos1.position, 1f);

        yield return new WaitForSecondsRealtime(0.1f);
        greenEgg.transform.DOMove(greenEggEndPos.position, 0.9f);

        yield return new WaitForSecondsRealtime(1f);

        greenEgg.SetActive(false);

        yield return new WaitForSecondsRealtime(1f);

        player.transform.position = new Vector3(17f, 3f, 100f);
        // player.transform.localScale = new Vector3(10f, 10f, 1f);
        player.transform.localScale = new Vector3(40f, 40f, 1f);

        player.transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)  // 線形のイージングを使用して一定速度で回転
            .SetLoops(-1, LoopType.Incremental);  // 無限にループさせる
        player.GetComponent<SpriteRenderer>().sortingOrder = 0;
        player.transform.DOPath(new Vector3[] { playerMovePos2.position, playerMovePos3.position }, 1.5f, PathType.CatmullRom);
        yield return new WaitForSecondsRealtime(1.1f);

        player.GetComponent<SpriteRenderer>().sortingOrder = 2;
        yield return new WaitForSecondsRealtime(0.2f);

        player.transform.DOScale(1000f, 1.2f);

        player.transform.DOMove(playerEndPos.position, 1f);


        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("ED3");
    }

}
