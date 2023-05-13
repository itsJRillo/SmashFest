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
    private string displayName;

    public TMP_InputField roomInput;
    public GameObject lobbyPanel;
    public GameObject roomPanel;
    public TMP_Text roomName;
    public TMP_Text username;
    public GameObject playButton;

    // GameObject of the room that is created
    public RoomLabel roomPrefab;
    List<RoomLabel> listRooms = new List<RoomLabel>();
    public Transform contentList;

    // GameObject of the player that entered the room
    public CardRoom playerPrefab;
    List<CardRoom> listPlayers = new List<CardRoom>();
    public Transform contentPlayers;

    float updateTime = 1.5f;

    void Awake() {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            status = task.Result;
            if (status == DependencyStatus.Available) {
                InitializeFirebase();
            } 
        });
    }

    void Update() {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2){
            playButton.SetActive(true);
        } else {
            playButton.SetActive(true);
        }
    }

    public void PlayGame() {
        PhotonNetwork.LoadLevel("MultiplayerGame");
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
            displayName = user.DisplayName;
            username.text = "username: " + displayName;
        } else {
            Debug.Log("Not");
        }
    }

    public void OnClickCreate(){
        if(roomInput.text.Length >= 1) {
            PhotonNetwork.CreateRoom(roomInput.text, new RoomOptions() { 
                MaxPlayers = 2, BroadcastPropsChangeToAll = true
            });
        }
    }

    public override void OnJoinedRoom() {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room name:" + PhotonNetwork.CurrentRoom.Name;
        UpdatePlayerList();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {

        if(Time.time >= updateTime){
            UpdateRoomList(roomList);
            updateTime = Time.time + 1.5f;
        }

    }

    public override void OnPlayerEnteredRoom(Player newPlayer) {
        UpdatePlayerList();
    }

    public override void OnPlayerLeftRoom(Player other) {
        UpdatePlayerList();
    }

    public void UpdateRoomList(List<RoomInfo> roomList) {
        foreach (RoomLabel item in listRooms) {
            Destroy(item.gameObject);
        }
        listRooms.Clear();

        foreach(RoomInfo room in roomList) {
            RoomLabel newRoom = Instantiate(roomPrefab, contentList);
            newRoom.SetRoomName(room.Name);
            listRooms.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnClickLeaveRoom() {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom() {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public void UpdatePlayerList() {
        foreach (CardRoom item in listPlayers) {
            Destroy(item.gameObject);
        }
        listPlayers.Clear();

        if(PhotonNetwork.CurrentRoom == null){
            return;
        }

        foreach(KeyValuePair<int, Player> player in PhotonNetwork.CurrentRoom.Players) {
            CardRoom newPlayer = Instantiate(playerPrefab, contentPlayers);
            newPlayer.SetPlayerName(displayName);
            newPlayer.SetPlayer(player.Value);

            if(player.Value == PhotonNetwork.LocalPlayer) {
                newPlayer.ApplyChanges();
            }
            listPlayers.Add(newPlayer);
        }
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public void goToMain() {
        SceneManager.LoadScene("MainScene");
    }
}
