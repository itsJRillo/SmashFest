using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfigurationManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDropdown;
    

    private Resolution[] resolutions;

    // Método para cambiar la resolución del viewport
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    private void Start()
    {
        // Load the configuration values when the scene starts
        LoadConfiguration();

        // Viewport configuration
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        // Obtén las resoluciones disponibles y añádelas al Dropdown
        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            // Comprueba si la resolución actual coincide con la resolución de pantalla
            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add listeners to the toggle and slider
        fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    // Method called when the fullscreen toggle value changes
    private void OnFullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    // Method called when the volume slider value changes
    private void OnVolumeSliderChanged(float volume)
    {
        AudioListener.volume = volume;
    }

    // Save the configuration values
    private void SaveConfiguration()
    {
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    // Load the configuration values
    private void LoadConfiguration()
    {
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;

        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;
        AudioListener.volume = volume;
    }

    // Method called when the scene is unloaded
    private void OnDestroy()
    {
        // Save the configuration values when the scene is unloaded
        SaveConfiguration();
    }
}

