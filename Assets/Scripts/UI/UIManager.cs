using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenuUI;

    public GameObject leaderboardUI;

    public GameObject configurationUI;

    private void Awake()
    {
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

    public void StartMenuScreen()
    {
        startMenuUI.SetActive(true);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(false);
    }

    public void LeaderboardScreen()
    {
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(true);
        configurationUI.SetActive(false);
    }

    public void ConfigurationScreen()
    {
        startMenuUI.SetActive(false);
        leaderboardUI.SetActive(false);
        configurationUI.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
