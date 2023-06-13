using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovieSE : MonoBehaviour
{
    public AudioClip chargingSE;
    AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public IEnumerator PlayChargingSE(float duration)
    {
        source.clip = chargingSE;
        source.pitch = 0.8f;
        source.PlayOneShot(chargingSE);
        yield return new WaitForSeconds(duration);

        source.Stop();
    }
}
