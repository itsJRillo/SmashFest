using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float timeRemaining = 120.0f;
    public static bool gameOver = false;
    public TMP_Text timerText;

    void Update() {
        timeRemaining -= Time.deltaTime;
        if (timeRemaining <= 0.0f || gameOver == true) {
            timerText.text = "Time's up";
        }
        int minutes = Mathf.FloorToInt(timeRemaining / 60.0f);
        int seconds = Mathf.FloorToInt(timeRemaining % 60.0f);
    
        timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
    }


}
