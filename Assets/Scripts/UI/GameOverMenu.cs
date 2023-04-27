using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {
    public static bool GameIsPaused = false;
    public GameObject gameOverMenuUI;

    void Update() {
        if(CountdownTimer.gameOver){
            Pause();
        }
    }

    public void Resume() {
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause() {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void LoadCharacterSelection() {
        Debug.Log("Loading character selection...");
        CountdownTimer.gameOver = false;
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SceneManager.LoadScene("CharacterSelection");
    }

    public void PlayAgain() {
        Debug.Log("Restarting game...");
        CountdownTimer.gameOver = false;
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
