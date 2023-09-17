using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpecialGun : MonoBehaviour
{
    [SerializeField] Transform StartPos;
    [SerializeField] Transform EndPos;
    [SerializeField] Stage5Manager stage5Manager;

    private void Start()
    {
        transform.position = StartPos.position;
        transform.DOMove(EndPos.position, 7.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Player3DController>().isSpecialGun = true;
            stage5Manager.DisplayReleaseText();
            this.gameObject.SetActive(false);
        }
    }
}
