using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;


public class CardRoom : MonoBehaviourPunCallbacks {
    public TMP_Text playerName;
    Image background;
    public Color highlight;

    public GameObject leftArrow;
    public GameObject rightArrow;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();
    public Image avatar;
    public Sprite[] characters;

    Player player;

    private void Awake() {
        background = GetComponent<Image>();
    }

    public void SetPlayer(Player _player) {
        playerName.text = _player.NickName;
        player = _player;
        UpdatePlayerCard(player);
    }

    public void ApplyChanges() {
        background.color = highlight;
        leftArrow.SetActive(true);
        rightArrow.SetActive(true);
    }

    public void OnClickLeftArrow() {
        if((int) playerProperties["playerCharacter"] == 0){
            playerProperties["playerCharacter"] = characters.Length - 1;
        } else {
            playerProperties["playerCharacter"] = (int) playerProperties["playerCharacter"] - 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow() {
        if((int) playerProperties["playerCharacter"] == characters.Length - 1){
            playerProperties["playerCharacter"] = 0;
        } else {
            playerProperties["playerCharacter"] = (int) playerProperties["playerCharacter"] + 1;
        }
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable properties) {
        if(player == target) {
            UpdatePlayerCard(target);
        }
    }

    public void UpdatePlayerCard(Player target) {
        if(target.CustomProperties.ContainsKey("playerCharacter")) {
            avatar.sprite = characters[(int) target.CustomProperties["playerCharacter"]];
            playerProperties["playerCharacter"] = (int) target.CustomProperties["playerCharacter"];
        } else {
            playerProperties["playerCharacter"] = 0;
        }
    }
}
