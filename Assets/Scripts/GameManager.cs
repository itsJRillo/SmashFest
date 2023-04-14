using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;
    public Transform spawnPoint;

    // Start is called before the first frame update
    void Start() {
        int characterSelected = PlayerPrefs.GetInt("characterSelected");
        GameObject prefab = characters[characterSelected];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
    }
}
