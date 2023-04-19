using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStatus : MonoBehaviour {

    public float enemyHealth = 100;
    public float maxHealth;
    public float damage;

    public Image healthBar;
    // public Animator animator;

    // Start is called before the first frame update
    void Start(){
        maxHealth = enemyHealth;
    }

    // Update is called once per frame
    void Update(){
        healthBar.fillAmount = Mathf.Clamp(enemyHealth / maxHealth, 0, 1);
    }

    public void OnCollisionEnter2D(Collision2D other) {
        PlayerStatus player = other.gameObject.GetComponent<PlayerStatus>();
        if(other.gameObject.CompareTag("Player")){

            player.playerHealth -= damage;
            Debug.Log("Enemy hit");
            player.animator.SetBool("hitted", true);

            if (player.playerHealth < 0) {
                player.animator.SetBool("isDead", true);
                StartCoroutine(player.Dead());
                CountdownTimer.gameOver = true;
            }

        }
        player.animator.SetBool("hitted", false);
    }
}
