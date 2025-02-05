using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage1OPSecondSM : MonoBehaviour
{

    public Transform pepeStopPosition;
    public Transform pepeOutPosition;
    public GameObject Player;
    public GameObject pepeFire;
    public Transform[] backgrounds; // 背景のレイヤーを格納する配列
    public float[] speeds; // それぞれの背景の移動速度を格納する配列
    public Vector2 direction = new Vector2(-1, -1f); // 移動する方向（左斜め下）

    public bool isMove = false;
    public Image blackoutPanel;


    // Start is called before the first frame update
    void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        pepeFire.SetActive(false);
        StartCoroutine(StartScene());
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove == true)
        {
            ParallaxScrolling();
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    public IEnumerator StartScene()
    {
        yield return new WaitForSeconds(1f);
        Player.transform.DOMove(pepeStopPosition.position, 1f);
        Player.transform.DOScale(new Vector3(200f, 200f, 0), 1f);
        yield return new WaitForSeconds(0.3f);
        isMove = true;
        yield return new WaitForSeconds(3.4f);

        pepeFire.SetActive(true);

        Player.transform.DOShakeScale(1.5f, 50f, 60, 90f, false);

        yield return new WaitForSeconds(1.5f);

        Player.transform.DOShakePosition(2f, 100f, 10, 100, false, false);
        pepeFire.transform.DOScale(new Vector3(0f, 0f, 0f), 2f);
        yield return new WaitForSeconds(2f);
        pepeFire.SetActive(false);
        yield return new WaitForSeconds(1f);
        isMove = false;
        Player.transform.DOMove(pepeOutPosition.position, 2f);
        Player.transform.DOScale(new Vector3(0, 0, 0), 2f);

        yield return new WaitForSeconds(4f);
        StartCoroutine(SkipScene());
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

    IEnumerator SkipScene()
    {
        BGMManager.instance.StopBGM();
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Stage1");
    }
}

