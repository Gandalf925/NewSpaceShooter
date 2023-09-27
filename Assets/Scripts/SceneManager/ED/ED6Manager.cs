using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class ED6Manager : MonoBehaviour
{
    public GameObject player;
    public Transform pepeStartPos;
    public Transform pepeStopPos;
    public Transform pepeEndPos;
    public Transform[] backgrounds; // 背景のレイヤーを格納する配列
    public float[] speeds; // それぞれの背景の移動速度を格納する配列
    public Vector2 direction = new Vector2(-1, -1f); // 移動する方向（左斜め下）

    public bool isMove = false;



    public Image blackoutPanel;

    [SerializeField] AudioClip ED3BGM;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        player.transform.position = pepeStartPos.position;
        StartCoroutine(ED6());
    }
    private void Update()
    {
        if (isMove == true)
        {
            ParallaxScrolling();
        }
    }

    IEnumerator ED6()
    {
        // BGMManager.instance.PlayBGM(stage5OP1BGM);
        yield return new WaitForSecondsRealtime(1f);
        isMove = true;

        yield return new WaitForSecondsRealtime(1f);

        player.transform.DOMove(pepeStopPos.position, 1f);
        player.transform.DORotate(new Vector3(0, 0, 360), 0.7f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)  // 線形のイージングを使用して一定速度で回転
            .SetLoops(-1, LoopType.Incremental);  // 無限にループさせる

        yield return new WaitForSecondsRealtime(3.5f);

        isMove = false;

        player.transform.DOScale(new Vector3(0f, 0f, 0), 1f);
        yield return new WaitForSecondsRealtime(2f);

        SceneManager.LoadScene("ED7");
    }

    private void ParallaxScrolling()
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            // 背景の新しい位置を計算
            Vector3 newPos = backgrounds[i].position + (Vector3)(direction * speeds[i] * Time.deltaTime);

            // 背景の位置を更新
            backgrounds[i].position = newPos;

            if (isMove == false)
            {
                break;
            }
        }
    }

}
