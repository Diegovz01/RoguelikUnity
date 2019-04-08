using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource sfxSource;

    public float lowPitchRange = 0.95f;
    public float highPitchRange = 1.05f;

    public static _SoundManager instance;

    private void Awake()
    {
        if (_SoundManager.instance == null)
        {
            _SoundManager.instance = this;
        }
        else if(_SoundManager.instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySingle(AudioClip clip)
    {
        sfxSource.pitch = 1f; // pitch = tono
        sfxSource.clip = clip;
        sfxSource.Play();
    }

    public void RandomizeSfx(params AudioClip[] clips) // params => colocar tantos clips (sin usar new Audio...)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);
        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }
}
