using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public AudioSource musicSource;
    public Slider musicSlider;
    public static float musicVolume = 1;

    public AudioSource sfxSource;
    public Slider sfxSlider;
    public static float sfxVolume = 1;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.volume = musicVolume;
        musicSlider.value = musicVolume;

        sfxSource.volume = sfxVolume;
        sfxSlider.value = sfxVolume;
    }

    public void UpdateMusicVolume()
    {
        musicVolume = musicSlider.value;
        musicSource.volume = musicSlider.value;
    }

    public void UpdateSFXVolume()
    {
        sfxVolume = sfxSlider.value;
        sfxSource.volume = sfxSlider.value;
    }
}
