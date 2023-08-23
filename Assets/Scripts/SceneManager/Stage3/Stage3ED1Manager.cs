using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class Stage3ED1Manager : MonoBehaviour
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
    public Transform map;
    public Transform mapStopPos;
    public GameObject backgroundPanel;

    [Header("Sounds")]
    [SerializeField] AudioSource seManager;
    [SerializeField] AudioClip cheerSE;

    [SerializeField] AudioClip Stage3ED1BGM;


    public Image blackoutPanel;

    private void Start()
    {
        blackoutPanel.color = new Color(0f, 0f, 0f, 0f);
        StartCoroutine(Stage3ED1());
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SkipScene());
        }
    }

    IEnumerator Stage3ED1()
    {

        BGMManager.instance.PlayBGM(Stage3ED1BGM);
        seManager.PlayOneShot(cheerSE);

        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(cheerPepes(front1));
        StartCoroutine(cheerPepes(front2));
        StartCoroutine(cheerPepes(front3));
        StartCoroutine(cheerPepes(back1));
        StartCoroutine(cheerPepes(back2));
        StartCoroutine(cheerPepes(CEO));
        StartCoroutine(cheerPepes(bartender));


        yield return new WaitForSecondsRealtime(2f);

        StartCoroutine(PepeAnimationSequence());

        yield return new WaitForSecondsRealtime(1f);

        StartCoroutine(MapAnimationSequence());

        yield return new WaitForSecondsRealtime(3f);

        SceneManager.LoadScene("Stage3ED2");
    }


    IEnumerator SkipScene()
    {
        blackoutPanel.DOFade(1f, 2f);
        yield return new WaitForSeconds(2f);
        BGMManager.instance.StopBGM();
        SceneManager.LoadScene("Stage4");
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

    IEnumerator PepeAnimationSequence()
    {
        // Pepeの初期位置に移動
        player.transform.position = pepeStartPos.transform.position;

        // パスを定義してアニメーション
        Vector3[] path = new Vector3[] { pepeStartPos.position, pepeTurnPos.position, pepeStopPos.position };
        player.transform.DOPath(path, 3f, PathType.CatmullRom);
        yield return new WaitForSeconds(1f);

        // 大きくなるアニメーション
        player.transform.DOScale(500f, 1f);

        // 待機
        yield return new WaitForSeconds(1f);

        // 消えるアニメーション
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        Color startColor = spriteRenderer.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        spriteRenderer.DOColor(endColor, 0.5f);

        // 待機
        yield return new WaitForSeconds(0.5f);

        // オブジェクトを破棄
        Destroy(player.gameObject);
    }

    IEnumerator MapAnimationSequence()
    {
        map.DORotate(new Vector3(0f, 0f, 90f), 1f);
        map.DOMove(mapStopPos.position, 0.6f);

        yield return new WaitForSeconds(0.8f);
        Destroy(map.gameObject);
    }
}
