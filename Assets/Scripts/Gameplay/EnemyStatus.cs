using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyStatus : MonoBehaviour {

    public Animator animator;
    public float enemyHealth = 100;
    public float maxHealth;
    public float damage;

    public Image healthBar;

    void Start(){
        maxHealth = enemyHealth;
    }

    void Update(){
        healthBar.fillAmount = Mathf.Clamp(enemyHealth / maxHealth, 0, 1);
    }

    public void OnCollisionEnter2D(Collision2D other) {
        PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
        if(other.gameObject.CompareTag("Player")){

            player.playerHealth -= damage;
            player.animator.SetTrigger("hitted");

            if (player.playerHealth < 0) {
                player.animator.SetTrigger ("isDead");
                CountdownTimer.gameOver = true;          
            }
        }
    }
}
