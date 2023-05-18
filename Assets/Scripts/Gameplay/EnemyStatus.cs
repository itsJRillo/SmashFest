using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class EnemyStatus : MonoBehaviour {

    public Animator animator;
    public float enemyHealth = 100;
    public float maxHealth;

    public Image healthBar;

    void Start(){
        maxHealth = enemyHealth;
    }

    void Update(){
        healthBar.fillAmount = Mathf.Clamp(enemyHealth / maxHealth, 0, 1);
    }
}
