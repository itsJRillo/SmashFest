using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingManager : MonoBehaviour
{
    public GameObject[] characters;
    public Transform spawnPoint;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start() {
        int characterSelected = PlayerPrefs.GetInt("characterSelected");
        GameObject prefab = characters[characterSelected];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        PlayerStatus playerStatus = clone.GetComponent<PlayerStatus>();
        playerStatus.healthBar = canvas.transform.Find("HealthBar_Player").GetComponent<Image>();
    }
}
