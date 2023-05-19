using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WarningCollider : MonoBehaviour
{
    private Stage2Manager stage2Manager;

    private void Start()
    {
        stage2Manager = FindObjectOfType<Stage2Manager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && stage2Manager.isBossBattle == false)
        {
            StartCoroutine(stage2Manager.WarningBeforBossBattle());
        }
    }
}
