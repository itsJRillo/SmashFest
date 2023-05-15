using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject menuUI;
    public GameObject startMenuUI;
    public GameObject leaderboardUI;
    public GameObject configurationUI;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void MenuScreen() {
        menuUI.SetActive(true);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void StartMenuScreen() {
        menuUI.SetActive(false);
        startMenuUI.SetActive(true);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void LeaderboardScreen() {
        menuUI.SetActive(false);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(true);
        configurationUI.SetActive(false);
    }

    public void ConfigurationScreen() {
        menuUI.SetActive(false);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
