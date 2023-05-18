using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
    public void ChangeScene() {
        SceneManager.LoadScene("MainScene");
    }
}
