using UnityEngine;
using Pathfinding;

using System.Collections;
using System.Linq;

public class EnemyMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public AIPath aiPath;
    public Animator animator;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform controllerHit;
    [SerializeField] private Vector2 sizeHit;
    [SerializeField] private float damageHit;


    IEnumerator death(Transform player) {
        player.GetComponent<PlayerStatus>().animator.SetTrigger("isDead");

        yield return new WaitForSeconds (3);

        CountdownTimer.gameOver = true;
        Time.timeScale = 0f;
    }

    IEnumerator hit(Transform player) {
        player.transform.GetComponent<PlayerStatus>().animator.SetTrigger("hitted");
        player.transform.GetComponent<PlayerStatus>().playerHealth -= damageHit;

        yield return new WaitForSeconds (2);
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(controllerHit.position, sizeHit);    
    }


    void Update() {
        
        animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.magnitude));

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector3(1f,1f,1f);

        } else if (aiPath.desiredVelocity.x <= -0.01f){
            transform.localScale = new Vector3(-1f,1f,1f);
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStatus playerStatus;

        if (player.TryGetComponent(out playerStatus)) {
            if (Vector2.Distance(transform.position, player.transform.position) < 2f) {
                Debug.Log("In range");
                animator.SetTrigger("isAttacking");
                hit(player.transform);

                if (playerStatus.playerHealth <= 0) {
                    StartCoroutine(death(player.transform));
                }
            }
        }

        
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
