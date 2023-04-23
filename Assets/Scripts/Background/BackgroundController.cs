using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    public Transform backgroundFront;
    public Transform backgroundBack;

    public float scrollSpeedFront = 0.1f;
    public float scrollSpeedBack = 0.05f;

    private List<Transform> starsFront = new List<Transform>();
    private List<Transform> starsBack = new List<Transform>();

    private PlayerController playerController;

    private void Start()
    {
        // 近景の星オブジェクトをリストに格納する
        foreach (Transform star in backgroundFront)
        {
            starsFront.Add(star);
        }

        // 遠景の星オブジェクトをリストに格納する
        foreach (Transform star in backgroundBack)
        {
            starsBack.Add(star);
        }

        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        // 星をスクロールする
        foreach (Transform star in starsFront)
        {
            float movement = playerController ? playerController.transform.position.y * -0.02f : 0f;
            star.position += new Vector3(-scrollSpeedFront, movement, 0) * Time.deltaTime;

            // 星が画面左に出たら、右に移動する
            if (star.position.x < -22f)
            {
                star.position += new Vector3(44f, 0, 0);
            }
        }

        foreach (Transform star in starsBack)
        {
            star.position += new Vector3(-scrollSpeedBack, 0, 0) * Time.deltaTime;

            // 星が画面左に出たら、右に移動する
            if (star.position.x < -22f)
            {
                star.position += new Vector3(44f, 0, 0);
            }
        }
    }
}