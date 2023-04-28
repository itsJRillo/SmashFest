using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {

    public float playerHealth = 100;
    public float maxHealth;

    public Image healthBar;
    public Animator animator;

    // Start is called before the first frame update
    void Start(){
        maxHealth = playerHealth;
    }

    // Update is called once per frame
    void Update(){
        healthBar.fillAmount = Mathf.Clamp(playerHealth / maxHealth, 0, 1);
    }

}