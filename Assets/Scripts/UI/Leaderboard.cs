using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Leaderboard : MonoBehaviour
{
    public TMP_Text nom;
    public TMP_Text victorias;

    public void SetLeaderboardUser(string _nom, int _victorias){
        nom.text = _nom;
        victorias.text = _victorias.ToString();
    }
}
