using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour {
    public GameObject gameOverMenuUI;

    void Update() {
        if(CountdownTimer.gameOver || CountdownTimer.timeRemaining <= 0.0f){
            Pause();
        } else {
            if(PauseMenu.GameIsPaused == true){
                Time.timeScale = 0f;
            } else {
                Resume();
            }
        }
    }

    public void Resume() {
        Time.timeScale = 1f;
        gameOverMenuUI.SetActive(false);
    }

    public void Pause() {
        Time.timeScale = 0f;
        gameOverMenuUI.SetActive(true);
    }

    public void LoadCharacterSelection() {
        Debug.Log("Loading character selection...");
        CountdownTimer.gameOver = false;
        CountdownTimer.timeRemaining = 120f;
        
        Time.timeScale = 1f;
        gameOverMenuUI.SetActive(false);

        SceneManager.LoadScene("CharacterSelection");
    }

    public void PlayAgain() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        CountdownTimer.gameOver = false;
        CountdownTimer.timeRemaining = 180f;
        
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }
}
