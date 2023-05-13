using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour {
    public GameObject[] characters;
    public int characterSelected = 0;

    // Start is called before the first frame update
    public void NextCharacter() {
        characters[characterSelected].SetActive(false);
        characterSelected = (characterSelected + 1) % characters.Length;
        characters[characterSelected].SetActive(true);
    }

    public void PreviousCharacter() {
        characters[characterSelected].SetActive(false);
        characterSelected--;
        if(characterSelected < 0){
            characterSelected += characters.Length;
        }
        characters[characterSelected].SetActive(true);
    }

    public void StartGame(){
        PlayerPrefs.SetInt("characterSelected", characterSelected);
        SceneManager.LoadScene("ForestScene");
    }
}
