using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeFireShotSE : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource seSource;
    public AudioClip shotSE;
    // Start is called before the first frame update
    void Start()
    {
        seSource = GetComponent<AudioSource>();
        seSource.PlayOneShot(shotSE);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
