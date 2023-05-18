using System.Collections;
using System.Collections.Generic;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerManager : MonoBehaviour
{
    public GameObject[] characters;

    public Transform[] spawnPoints;

    private DependencyStatus status;

    private FirebaseAuth auth;

    private FirebaseUser Usuari;

    private DatabaseReference db;

    private PhotonView view;

    public Canvas canvas;

    void Awake()
    {
        FirebaseApp
            .CheckAndFixDependenciesAsync()
            .ContinueWith(task =>
            {
                status = task.Result;
                if (status == DependencyStatus.Available)
                {
                    InitializeFirebase();
                }
            });
    }

    private void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        if (Application.isEditor)
        {
            database.SetPersistenceEnabled(false);
        }

        db = database.RootReference;
    }

    private void saveGame()
    {
        string jugador1 = PhotonNetwork.PlayerList[0].NickName;
        string jugador2 = PhotonNetwork.PlayerList[1].NickName;
        // string ganador = ganador;

        /*
        db.Child("partides").SetRawJsonValueAsync({"duració":"", "jugador1":jugador1, "jugador2":jugador2, "ganador":""}).ContinueWith(task => {
            if (task.IsCompleted) {
                Debug.Log("Resultados guardados en Firebase");
            } else {
                Debug.LogError("Error al guardar los resultados en Firebase: " + task.Exception);
            }
        });
        */
    }

    void Start()
    {
        int spawnIndex;

        if (!PhotonNetwork.IsMasterClient){
            spawnIndex = 1;
        } else {
            spawnIndex = 0;
        }

        Transform spawnPoint = spawnPoints[spawnIndex];
        GameObject prefab =
            characters[(int)
            PhotonNetwork.LocalPlayer.CustomProperties["playerCharacter"]];

        if(PhotonNetwork.IsMasterClient){
            prefab.GetComponent<PlayerStatus>().healthBar = canvas.transform.Find("HealthBar_Player").GetComponent<Image>();
        } else {
            prefab.GetComponent<PlayerStatus>().healthBar = canvas.transform.Find("HealthBar_Enemy").GetComponent<Image>();
        }

        prefab.transform.localScale = new Vector2(35f, 35f);
        PhotonNetwork
            .Instantiate(prefab.name,
            spawnPoint.position,
            Quaternion.identity);
    }
}
