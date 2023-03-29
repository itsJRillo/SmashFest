using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SmashOption : MonoBehaviour {
    // Update is called once per frame
    void Update() {
        // Check if the left mouse button is clicked
        if (Input.GetMouseButtonDown(0))
        {
            // Load the next scene
            SceneManager.LoadScene("ForestScene");
        }
    }
}
