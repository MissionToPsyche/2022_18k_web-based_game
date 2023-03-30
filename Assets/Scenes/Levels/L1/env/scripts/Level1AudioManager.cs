using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Level1AudioManager : MonoBehaviour
{
    private AudioSource audios;
    public AudioClip walking;
    public AudioClip land;
    public AudioClip jump;
    public AudioClip collected;
    public AudioClip death;
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    public void PlayWalk()
    {
        if (audios.isPlaying)
        {
            return;
        }
        audios.clip = walking;
        audios.volume = .75f;
        audios.pitch = .8f;
        audios.Play();
    }

    public void PlayJump()
    {
        audios.Stop();
        audios.clip = jump;
        audios.volume = 1f;
        audios.pitch = 1f;
        audios.Play();
    }

    public void PlayCollectible()
    {
        audios.Stop();
        audios.clip = collected;
        audios.pitch = 1f;
        audios.Play();
    }

    public void PlayDeath()
    {
        audios.Stop();
        audios.clip = death;
        audios.pitch = 1f;
        audios.volume = 1f;
        audios.Play();
    }
}
