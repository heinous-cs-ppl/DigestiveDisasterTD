using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    public static AudioSource musicSource;
    public static Slider volumeSlider;

    void Start() {
        musicSource = GameObject.Find("Music Source").GetComponent<AudioSource>();
        volumeSlider = this.gameObject.GetComponent<Slider>();
    }
    // update the volume of sound effects
    public static void UpdateMusicVolume()
    {
        musicSource.volume = volumeSlider.value;
    }
}
