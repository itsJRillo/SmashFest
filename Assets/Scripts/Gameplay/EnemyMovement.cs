using UnityEngine;
using Pathfinding;

public class EnemyMovement : MonoBehaviour
{
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    public AIPath aiPath;
    public Animator animator;
    private float attackDistance = 1f;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform controllerHit;
    [SerializeField] private Vector2 sizeHit;
    [SerializeField] private float damageHit;


    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")){
            animator.SetTrigger("isAttacking");
            collision.transform.GetComponent<PlayerStatus>().animator.SetTrigger("hitted");
            collision.transform.GetComponent<PlayerStatus>().playerHealth -= damageHit;
        }
    }


    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(controllerHit.position, sizeHit);    
    }


    void Update() {
        if(CountdownTimer.gameOver){
            animator.SetFloat("Speed", 0);
        } else {
            animator.SetFloat("Speed", Mathf.Abs(aiPath.desiredVelocity.magnitude));
        }

        if(aiPath.desiredVelocity.x >= 0.01f){
            transform.localScale = new Vector3(1f,1f,1f);

        } else if (aiPath.desiredVelocity.x <= -0.01f){
            transform.localScale = new Vector3(-1f,1f,1f);
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
}
