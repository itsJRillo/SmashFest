using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerManager : MonoBehaviour {
    public GameObject[] characters;
    public Transform[] spawnPoints;

    void Start() {
        int rand = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[rand];
        GameObject prefab = characters[(int) PhotonNetwork.LocalPlayer.CustomProperties["playerCharacter"]];
        PhotonNetwork.Instantiate(prefab.name, spawnPoint.position, Quaternion.identity);
    }

}
