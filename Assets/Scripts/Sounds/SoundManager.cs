using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmSource;
    public AudioSource seSource;

    public AudioClip explosionSE;
    public AudioClip warningSE;

    public static SoundManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // BGMの再生
    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.pitch = 1f;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlaySE(AudioClip seClip)
    {
        seSource.clip = seClip;
        seSource.pitch = 1f;
        seSource.PlayOneShot(seClip);
    }

    public void PlayExplosionSE()
    {
        seSource.clip = explosionSE;
        seSource.pitch = 1.2f;
        seSource.PlayOneShot(explosionSE);
    }

    public IEnumerator PlayWarningSE(float duration)
    {

        seSource.clip = warningSE;
        seSource.pitch = 0.8f;
        seSource.PlayOneShot(warningSE);
        yield return new WaitForSeconds(duration);

        seSource.Stop();
    }

    public IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / duration;
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void ResumeBGM()
    {
        bgmSource.UnPause();
    }
}
