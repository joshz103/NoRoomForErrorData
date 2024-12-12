using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class Settings : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    public void ChangeMasterVolume()
    {
        PlayerPrefs.SetFloat("mastervolume", masterVolumeSlider.value);
        audioMixer.SetFloat("Master", Mathf.Log10(masterVolumeSlider.value / 100) * 20);
    }

    public void ChangeMusicVolume()
    {
        PlayerPrefs.SetFloat("musicvolume", musicVolumeSlider.value);
        audioMixer.SetFloat("Music", Mathf.Log10(musicVolumeSlider.value / 100) * 20);
    }

    public void ApplySettings()
    {
        audioMixer.SetFloat("Master", PlayerPrefs.GetInt("mastervolume"));
        audioMixer.SetFloat("Music", PlayerPrefs.GetInt("musicvolume"));
    }
}
