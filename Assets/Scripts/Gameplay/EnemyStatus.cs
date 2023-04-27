using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
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
            StartCoroutine(hitted(player.animator));

            if (player.playerHealth < 0) {
                StartCoroutine(death(player.animator));
                
            }
        }
    }

    private IEnumerator hitted(Animator animator) {
        animator.SetTrigger ("hitted");
        yield return new WaitForSeconds (2);
    }

    private IEnumerator death(Animator animator) {
        animator.SetTrigger ("isDead");
        yield return new WaitForSeconds (2);
        
        CountdownTimer.gameOver = true;
        Time.timeScale = 0f;
    }
}
