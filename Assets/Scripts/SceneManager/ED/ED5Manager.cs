using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ED5Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeEndPos;

    public Transform stage1Boss;

    public Transform bossStopPos;
    public GameObject speechBubble;



    public Image blackoutPanel;

    [SerializeField] AudioClip ED3BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        player.transform.position = pepeStartPos.position;
        speechBubble.transform.localScale = new Vector3(0f, 0f, 0f);
        StartCoroutine(ED5());
    }
    private void Update()
    {

    }

    IEnumerator ED5()
    {
        // BGMManager.instance.PlayBGM(stage5OP1BGM);
        yield return new WaitForSecondsRealtime(1f);

        stage1Boss.transform.DOMove(bossStopPos.position, 4f);

        yield return new WaitForSecondsRealtime(3f);

        player.SetActive(true);

        player.transform.DOMove(pepeEndPos.position, 3f);
        player.transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)  // 線形のイージングを使用して一定速度で回転
            .SetLoops(-1, LoopType.Incremental);  // 無限にループさせる
        player.transform.DOScale(0f, 3f);

        yield return new WaitForSecondsRealtime(3f);

        speechBubble.transform.DOScale(new Vector3(0.25f, 0.1f, 0f), 1f);

        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("ED5");
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
