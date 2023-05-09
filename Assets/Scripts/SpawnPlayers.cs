using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour {
    public GameObject player;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    void Start() {
        Vector2 randomPosition = new Vector2(Random.Range(minX, maxX), Random.Range(minY, maxY));
        player = GameObject.FindWithTag("Player");
        PhotonNetwork.Instantiate(player.name, randomPosition, Quaternion.identity);
    }


}
