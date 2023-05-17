using UnityEngine;
using Pathfinding;

using System.Collections;
using System.Linq;

public class EnemyMovement : MonoBehaviour {
    
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform controllerHit;
    [SerializeField] private Vector2 sizeHit;
    [SerializeField] private float damageHit;

    private bool isAttacking = false;
    private bool isDead = false;
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public AIPath aiPath;
    public Animator animator;

    private void hit() {
        if(!isAttacking) return;

        Collider2D[] obj = Physics2D.OverlapBoxAll(controllerHit.position, sizeHit, 0f);

        foreach (Collider2D item in obj) {
            if(item.CompareTag("Player")){
                StartCoroutine(ApplyDamage(item.transform));
                animator.SetTrigger("isAttacking");

                item.transform.GetComponent<PlayerStatus>().animator.SetTrigger("hitted");
            }
        }
    }

    IEnumerator ApplyDamage(Transform playerTransform) {
        PlayerStatus player = playerTransform.GetComponent<PlayerStatus>();
        player.playerHealth -= damageHit;
        Debug.Log("Enemy inflicted " + damageHit + " damage to player. Player's current health: " + player.playerHealth);
        yield return null;
        if(player.playerHealth <= 0) {
            StartCoroutine(death(playerTransform));
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(controllerHit.position, sizeHit);    
    }

    void Update() {
        GameObject player = GameObject.FindWithTag("Player");

        float distance = Mathf.Abs(transform.position.x - player.transform.position.x);

        if(distance < 10f){
            if(!isAttacking){
                if(isDead){
                    Debug.Log("Player is dead");
                } else {
                    StartCoroutine(attack());
                }
            }
        } else {
            animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.magnitude));
        }

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector2(1.5f,1.5f);

        } else if (aiPath.desiredVelocity.x <= -0.01f){
            transform.localScale = new Vector2(-1.5f,1.5f);
        }
        Debug.Log("Desired Velocity: " + aiPath.desiredVelocity);
    }

    IEnumerator attack() {
        isAttacking = true;
        hit();
        Debug.Log("Enemy attacking player...");
        yield return new WaitForSeconds(1);
        isAttacking = false;
        Debug.Log("Enemy attack finished.");
    }

    IEnumerator death(Transform player) {
        isDead = true;
        player.GetComponent<PlayerStatus>().animator.SetTrigger("isDead");
        yield return new WaitForSeconds (3);
        CountdownTimer.gameOver = true;
        Time.timeScale = 0f;
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
