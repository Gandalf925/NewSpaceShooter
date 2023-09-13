using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Stage5Manager : MonoBehaviour
{
    [SerializeField] GameObject god;
    [SerializeField] GameObject godAngryMark;
    [SerializeField] GameObject SpecialGun;
    [SerializeField] Transform godStartPos;
    [SerializeField] Transform godEndPos;

    // void Start()
    // {

    // }

    // public IEnumerator StartLastEvent()
    // {

    // }

    public IEnumerator StartLastEvent()
    {
        yield return new WaitForSeconds(3f);
        god.SetActive(true);
        god.transform.DOShakePosition(2f, 1f, 100, 90f, false, true);
        god.transform.DOMoveY(godEndPos.position.y, 2f);

        yield return new WaitForSeconds(2f);
        godAngryMark.SetActive(true);
        Instantiate(SpecialGun, new Vector3(0, 300, 0), Quaternion.identity);

        yield return new WaitForSeconds(5f);
        god.transform.DOShakePosition(2f, 1f, 100, 90f, false, true);
        god.transform.DOMoveY(godStartPos.position.y, 2f);
        yield return new WaitForSeconds(3f);
        god.SetActive(false);

    }
}
