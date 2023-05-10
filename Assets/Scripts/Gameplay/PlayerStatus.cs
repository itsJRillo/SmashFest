using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

    public float playerHealth = 100;
    public float maxHealth;
    public int numberHits; // TODO: make an UI interface for the end of the game and add the number of hits

    public Image healthBar;
    public Animator animator;

    void Start(){
        maxHealth = playerHealth;
    }

    void Update(){
        healthBar.fillAmount = Mathf.Clamp(playerHealth / maxHealth, 0, 1);
    }

}
