using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource musicSource;
    public AudioClip mainMusic;
    public AudioClip bossMusic;
    public bool isBossMusicPlaying = false;

    private void Awake()
    {
        instance = this;
    }

    public void PlayMainMusic()
    {
        musicSource.clip = mainMusic;
        musicSource.Play();
        isBossMusicPlaying = false;
    }

    public void PlayBossMusic()
    {
        musicSource.clip = bossMusic;
        musicSource.Play();
        isBossMusicPlaying = true;
    }
}
