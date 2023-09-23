using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidExplosion : MonoBehaviour
{

    [Header("Audio")]
    public AudioSource seSource;
    public AudioClip explosionSE;
    // Start is called before the first frame update
    void Start()
    {
        seSource = GetComponent<AudioSource>();
        seSource.PlayOneShot(explosionSE);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
