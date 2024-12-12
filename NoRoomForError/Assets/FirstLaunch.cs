using UnityEngine;
using UnityEngine.UI;

public class FirstLaunch : MonoBehaviour
{
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            // First-time launch
            PlayerPrefs.SetFloat("mastervolume", 50);
            PlayerPrefs.SetFloat("musicvolume", 50);
            PlayerPrefs.SetString("FirstLaunch", "false");
            Debug.Log("First time launch settings applied");

            masterVolumeSlider.value = PlayerPrefs.GetFloat("mastervolume");
            musicVolumeSlider.value = PlayerPrefs.GetFloat("musicvolume");

            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("App has launched before!");
        }
    }

}