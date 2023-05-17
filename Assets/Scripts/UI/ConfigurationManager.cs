using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfigurationManager : MonoBehaviour
{
    public Slider volumeSlider;

    public Toggle fullscreenToggle;

    public TMP_Dropdown resolutionDropdown;

    public MusicShared musicShared;

    private Resolution[] resolutions;

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen
            .SetResolution(resolution.width,
            resolution.height,
            Screen.fullScreen);
    }

    private void Start()
    {
        LoadConfiguration();

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                resolutions[i].width + " x " + resolutions[i].height;
            options.Add (option);

            if (
                resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height
            )
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions (options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.onValueChanged.AddListener (OnFullscreenToggleChanged);

        musicShared = FindObjectOfType<MusicShared>();
        if (musicShared != null)
        {
            volumeSlider.value = musicShared.volume;
        }

        volumeSlider.onValueChanged.AddListener (OnVolumeSliderChanged);
    }

    private void OnFullscreenToggleChanged(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void OnVolumeSliderChanged(float volume)
    {
        AudioListener.volume = volume;

        if (musicShared != null)
        {
            musicShared.volume = volume;
            musicShared.AdjustVolume();
        }
    }

    private void SaveConfiguration()
    {
        PlayerPrefs.SetInt("Fullscreen", fullscreenToggle.isOn ? 1 : 0);
        PlayerPrefs.SetFloat("Volume", volumeSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadConfiguration()
    {
        bool fullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.isOn = fullscreen;
        Screen.fullScreen = fullscreen;

        float volume = PlayerPrefs.GetFloat("Volume", 0.5f);
        volumeSlider.value = volume;
        AudioListener.volume = volume;
    }

    private void OnDestroy()
    {
        SaveConfiguration();
    }
}
