using UnityEngine;

using System.Collections;
using System.Linq;
using Photon.Pun;

public class PlayerMovement : MonoBehaviour {
    private float horizontal;
    public float speed = 8f;
    public float jumpingPower = 16f;
    private bool isFacingRight = true;
    public Animator animator;
    PhotonView view;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private Transform controllerHit;
    [SerializeField] private Vector2 sizeHit;
    [SerializeField] private float damageHit;
    [SerializeField] private float attackCooldown;

    void Start() {
        view = GetComponent<PhotonView>();
    }

    private void hit() {
        Collider2D[] obj = Physics2D.OverlapBoxAll(controllerHit.position, sizeHit, 0f);

        foreach (Collider2D item in obj) {
            if(item.CompareTag("Enemy")){
                item.transform.GetComponent<EnemyStatus>().animator.SetTrigger ("hitted");
                item.transform.GetComponent<EnemyStatus>().enemyHealth -= damageHit;

                if(item.transform.GetComponent<EnemyStatus>().enemyHealth <= 0) {
                    StartCoroutine(death(item.transform));
                }
            }
        }
    }

    private IEnumerator death(Transform enemy) {
        enemy.GetComponent<EnemyMovement>().aiPath.canMove = false;
        enemy.GetComponent<EnemyStatus>().animator.SetTrigger("isDead");
        
        yield return new WaitForSeconds (3);
        
        enemy.GetComponent<BoxCollider2D>().size = new Vector3(0.3f,0,0);
        enemy.GetComponent<BoxCollider2D>().offset = new Vector2(-0.05f, -0.16f);
        CountdownTimer.gameOver = true;
        Time.timeScale = 0f;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(controllerHit.position, sizeHit);    
    }

    void Update() {
        horizontal = Input.GetAxisRaw("Horizontal");    

        if (Input.GetButtonDown("Jump") && IsGrounded()) {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f) {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            StartCoroutine(attack());
        } else if(Input.GetKeyUp(KeyCode.E)){
            animator.SetBool("isAttacking", false);
        }

        if(Input.GetKey(KeyCode.R)) {
            StartCoroutine(charge());
        } else if(Input.GetKeyUp(KeyCode.R)){
            animator.SetBool("Charging", false);
            hit();
        }

        Flip();
    }
    
    public IEnumerator attack(){
        animator.SetBool("isAttacking", true);
        yield return new WaitForSeconds(2);
    }

    public IEnumerator charge(){
        animator.SetBool("Charging", true);
        Animation anim = gameObject.GetComponent<Animation>();
        AnimationState animState = anim[anim.clip.name];

        if (anim.isPlaying) {
            if (animState.normalizedTime > 0) {
                hit();
            }
        }

        yield return new WaitForSeconds(2);
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private bool IsGrounded() {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip() {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f) {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
