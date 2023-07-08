using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour
{
    public AudioSource seSource;

    // BGMの再生
    public void PlaySE(AudioClip clip)
    {
        seSource.clip = clip;
        seSource.pitch = 1f;
        seSource.PlayOneShot(clip);
    }

    public void StopSE()
    {
        seSource.Stop();
    }
}
