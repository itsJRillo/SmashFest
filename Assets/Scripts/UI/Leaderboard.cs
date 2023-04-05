using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    private string nom;
    private int victorias;

    public void SetLeaderboardUser(string _nom, int _victorias){
        nom = _nom;
        victorias = _victorias;
    }
}
