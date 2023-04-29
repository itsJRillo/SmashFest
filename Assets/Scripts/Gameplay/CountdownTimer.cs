using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static float timeRemaining = 180.0f;
    public static bool gameOver = false;
    public TMP_Text timerText;

    void Update() {
        if (timeRemaining <= 0.0f) {
            timerText.text = "Game Over";
            Time.timeScale = 0f;
            
        } else {
            timeRemaining -= Time.deltaTime;

            int minutes = Mathf.FloorToInt(timeRemaining / 60.0f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60.0f);
        
            timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }


}
