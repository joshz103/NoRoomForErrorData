using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public bool isPaused = false;
    public GameObject fpsCounter;
    public GameObject DeathUIContainer;
    public GameObject optionsContainer;

    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;

    private bool optionsMenuOpen = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Pause") && DeathUIContainer.GetComponent<Canvas>().enabled == false && !optionsMenuOpen)
        {
            if (isPaused)
            {
                isPaused = false;
                PauseOff();
            }
            else
            {
                isPaused = true;
                PauseOn();
            }
        }

        if (Input.GetButtonDown("Pause") && optionsMenuOpen)
        {
            CloseOptions();
        }
    }

    private void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat("mastervolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicvolume");
    }

    public void PauseOn()
    {
        Time.timeScale = 0f;
        fpsCounter.SetActive(false);
        pauseMenuUI.SetActive(true);
        isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void PauseOff()
    {
        Time.timeScale = 1f;
        fpsCounter.SetActive(true);
        pauseMenuUI.SetActive(false);
        isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PauseRestartRound()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
        PauseOff();
    }

    public void PauseReturnToMainMenu()
    {
        PauseOff();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }

    public void OpenOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsContainer.SetActive(true);

        masterVolumeSlider.value = PlayerPrefs.GetFloat("mastervolume");
        musicVolumeSlider.value = PlayerPrefs.GetFloat("musicvolume");

        optionsMenuOpen = true;
    }

    public void CloseOptions()
    {
        pauseMenuUI.SetActive(true);
        optionsContainer.SetActive(false);
        optionsMenuOpen = false;
    }

}
