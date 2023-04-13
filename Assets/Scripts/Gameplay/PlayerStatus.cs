using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour {

    public int playerHealth;
    public Animator animator;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){

    }

    void OnTriggerEnter2D ( Collider2D col  ){
        playerHealth--;
        if (col.gameObject.tag == "Enemy") {
            if (playerHealth > 0) {
                animator.SetBool("isDead", true);
                StartCoroutine(Dead());
            }
        }
    }

    IEnumerator Dead() {
        Debug.Log ("dead");
        GetComponent<Renderer>().enabled = false;
        yield return new WaitForSeconds(5);
        Debug.Log ("respawn");
        GetComponent<Renderer>().enabled = true;
    }
    
}
