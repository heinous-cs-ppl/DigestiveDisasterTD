using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXVolume : MonoBehaviour
{
    public static AudioSource sfxSource;
    public static Slider volumeSlider;

    void Start() {
        sfxSource = GameObject.Find("SFX source").GetComponent<AudioSource>();
        volumeSlider = this.gameObject.GetComponent<Slider>();
    }
    // update the volume of sound effects
    public static void UpdateSFXVolume()
    {
        sfxSource.volume = volumeSlider.value;
    }
}
