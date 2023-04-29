using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class GameManager : MonoBehaviour
{
    public GameObject[] characters;
    public Transform spawnPoint;
    public Canvas canvas;
    public CountdownTimer timer;

    // Start is called before the first frame update
    void Start() {
        int characterSelected = PlayerPrefs.GetInt("characterSelected");
        GameObject prefab = characters[characterSelected];
        GameObject clone = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        PlayerStatus playerStatus = clone.GetComponent<PlayerStatus>();
        playerStatus.healthBar = canvas.transform.Find("HealthBar_Player").GetComponent<Image>();

        EnemyStatus enemyStatus = GameObject.FindWithTag("Enemy").GetComponent<EnemyStatus>();
        enemyStatus.healthBar = canvas.transform.Find("HealthBar_Enemy").GetComponent<Image>();

        AIDestinationSetter setterAI = GameObject.FindWithTag("Pathfinder").GetComponent<AIDestinationSetter>();
        setterAI.target = clone.transform;
    }
}
