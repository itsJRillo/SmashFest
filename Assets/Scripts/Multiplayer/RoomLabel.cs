using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomLabel : MonoBehaviour {
    public TMP_Text roomName;
    LobbyManager roomManager;

    void Start() {
        roomManager = FindObjectOfType<LobbyManager>();
    }

    public void SetRoomName(string _roomName) {
        roomName.text = _roomName;
    }

    public void OnClickItem() {
        roomManager.JoinRoom(roomName.text);
    }

    
}
