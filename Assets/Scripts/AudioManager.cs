using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource[] sfxSource;
    public Slider musicSlider;
    public Slider sfxSlider;

    private void Start()
    {
        musicSlider.value = musicSource.volume;
        musicSlider.onValueChanged.AddListener(SetMusicVolume);

        if (sfxSource.Length > 0)
        {
            sfxSlider.value = sfxSource[0].volume;
        }

        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicSource.volume = value;
    }

    public void SetSFXVolume(float value)
    {
        foreach (AudioSource sfx in sfxSource)
        {
            sfx.volume = value;
        }
    }
}
