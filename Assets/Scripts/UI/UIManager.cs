using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    //Screen object variables
    public GameObject loginUI;
    public GameObject registerUI;
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

    public void LoginScreen() {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
        menuUI.SetActive(false);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void RegisterScreen() {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
        menuUI.SetActive(false);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void MenuScreen() {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuUI.SetActive(true);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void StartMenuScreen() {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuUI.SetActive(false);
        startMenuUI.SetActive(true);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void LeaderboardScreen() {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuUI.SetActive(false);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(true);
        configurationUI.SetActive(false);
    }

    public void ConfigurationScreen() {
        loginUI.SetActive(false);
        registerUI.SetActive(false);
        menuUI.SetActive(false);
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(true);
    }
}
