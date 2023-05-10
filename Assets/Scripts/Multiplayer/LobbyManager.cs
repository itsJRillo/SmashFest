using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviourPunCallbacks {

    private DependencyStatus status;
    private DatabaseReference db;
    FirebaseAuth auth;
    FirebaseUser user;

    public TMP_InputField roomInput;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;
    public TMP_Text username;

    void Awake() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            status = task.Result;
            if (status == DependencyStatus.Available) {
                InitializeFirebase();
            } 
        });
    }

    private void InitializeFirebase() {
        auth = FirebaseAuth.DefaultInstance;
        var database = FirebaseDatabase.DefaultInstance;

        if (Application.isEditor) {
            database.SetPersistenceEnabled(false);
        }

        db = database.RootReference;
    }

    void Start() {
        PhotonNetwork.JoinLobby();

        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        if (user != null) {
            string displayName = user.DisplayName;
            username.text = "username: " + displayName;
        } else {
            Debug.Log("Not");
        }
    }

    public void OnClickCreate(){
        if(roomInput.text.Length >= 1) {
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions() { 
                MaxPlayers = 2 
            });
        }
    }

    public override void OnJoinedRoom() {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room name:" + PhotonNetwork.CurrentRoom.Name;
    }

    public void goToMain() {
        SceneManager.LoadScene("MainScene");
    }

}
