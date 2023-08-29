using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastBossController : MonoBehaviour
{

    public int maxHP = 1;
    private float currentHP;

    public GameObject player;
    public Transform[] BossClosePos;
    public Transform[] BossMidrangePos;
    public Transform[] BossFarPos;

    private bool isShowingDamage = false;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    GameManager gameManager;
    void Start()
    {
        transform.position = BossClosePos[0].position;
        StartCoroutine(BossChangePos());

        currentHP = maxHP;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        player = GameObject.FindWithTag("Player");
        // soundManager = GameObject.FindGameObjectWithTag("SoundManager").GetComponent<BGMManager>();
        gameManager = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            int damage = other.GetComponent<PlayerBulletController>().attackPower;
            currentHP -= damage;
            gameManager.UpdateScore(damage);

            // ダメージを受けた際の演出
            StartCoroutine(ShowDamageRoutine());

            Destroy(other.gameObject);
        }
    }

    IEnumerator BossChangePos()
    {
        for (int i = 0; i < BossClosePos.Length; i++)
        {
            transform.position = BossClosePos[i].position;
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < BossMidrangePos.Length; i++)
        {
            transform.position = BossMidrangePos[i].position;
            yield return new WaitForSeconds(1f);
        }

        for (int i = 0; i < BossFarPos.Length; i++)
        {
            transform.position = BossFarPos[i].position;
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator ShowDamageRoutine()
    {
        if (isShowingDamage) yield break;

        isShowingDamage = true;

        float blinkInterval = 0.07f;

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(blinkInterval);

        spriteRenderer.color = originalColor;

        isShowingDamage = false;
    }

}
