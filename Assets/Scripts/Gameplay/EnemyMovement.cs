using UnityEngine;
using Pathfinding;

using System.Collections;
using System.Linq;

public class EnemyMovement : MonoBehaviour
{
    private float horizontal;
    private bool isAttacking = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform controllerHit;
    [SerializeField] private Vector2 sizeHit;
    [SerializeField] private float damageHit;

    public float speed = 8f;
    public float jumpingPower = 16f;
    public AIPath aiPath;
    public Animator animator;

    private void hit() {
        if(!isAttacking) return;

        Collider2D[] obj = Physics2D.OverlapBoxAll(controllerHit.position, sizeHit, 0f);

        foreach (Collider2D item in obj) {
            if(item.CompareTag("Player")){
                item.transform.GetComponent<PlayerStatus>().animator.SetTrigger("hitted");
                
                animator.SetTrigger("isAttacking");
                StartCoroutine(ApplyDamage(item.transform));
            }
        }
    }

    IEnumerator ApplyDamage(Transform playerTransform) {
        PlayerStatus player = playerTransform.GetComponent<PlayerStatus>();
        player.playerHealth -= damageHit;
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
                StartCoroutine(attack());
            }
        } else {
            animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.magnitude));
        }

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector2(1f,1f);

        } else if (aiPath.desiredVelocity.x <= -0.01f){
            transform.localScale = new Vector2(-1f,1f);
        }  
    }

    IEnumerator attack() {
        isAttacking = true;
        hit();
        yield return new WaitForSeconds(1);
        isAttacking = false;
    }

    IEnumerator death(Transform player) {
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
