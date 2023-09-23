using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireStartSE : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource seSource;
    public AudioClip beepSE;
    // Start is called before the first frame update
    void Start()
    {
        seSource = GetComponent<AudioSource>();
        seSource.PlayOneShot(beepSE);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
