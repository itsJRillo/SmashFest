using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuHandler : MonoBehaviour
{
    public Image smashButton;
    public Image multiplayerButton;
    public Image optionsButton;

    void Start() {
        smashButton.GetComponent<Button>().onClick.AddListener(SmashClicked);
        multiplayerButton.GetComponent<Button>().onClick.AddListener(MultiplayerClicked);
        optionsButton.GetComponent<Button>().onClick.AddListener(OptionsClicked);
    }

    void SmashClicked() {
        // Load the Smash scene
        // SceneManager.LoadScene("ForestScene");
    }

    void MultiplayerClicked() {
        // Load the Multiplayer scene
        Debug.Log("Loads Multiplayer Scene");
    }

    void OptionsClicked() {
        // Load the Options scene
        Debug.Log("Loads Options Scene");
    }

    void LDButtonClicked() {
        // Load the leaderboard scene
        Debug.Log("Loads Leaderboard Scene");
    }
}
