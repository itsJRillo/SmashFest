using System.Collections;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviourPunCallbacks, IPunObservable
{
    PhotonView view;

    private Vector3 lastPosition;

    private bool lastIsAttacking;

    private bool lastIsCharging;

    private bool isAttacking = false;

    private bool isCharging = false;

    public AudioClip hitSound;

    public AudioSource audio;

    private float horizontal;

    public float speed = 8f;

    public float jumpingPower = 16f;

    private bool isFacingRight = true;

    public Animator animator;

    public Canvas canvas;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform controllerHit;

    [SerializeField]
    private Vector2 sizeHit;

    [SerializeField]
    private float damageHit;

    [SerializeField]
    private float attackCooldown;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        if (audio == null) audio = gameObject.AddComponent<AudioSource>();

        if (PhotonNetwork.IsConnected)
        {
            view = GetComponent<PhotonView>();
            canvas = GameObject.FindWithTag("Canvas").GetComponent<Canvas>();
            if (!view.IsMine)
            {
                rb.bodyType = RigidbodyType2D.Static;

                PlayerStatus playerStatus = GetComponent<PlayerStatus>();
                playerStatus.healthBar =
                    canvas
                        .transform
                        .Find("HealthBar_Enemy")
                        .GetComponent<Image>();
            }
            else
            {
                PlayerStatus playerStatus = GetComponent<PlayerStatus>();
                playerStatus.healthBar =
                    canvas
                        .transform
                        .Find("HealthBar_Player")
                        .GetComponent<Image>();
            }
        }
    }

    private void hit()
    {
        Collider2D[] obj =
            Physics2D.OverlapBoxAll(controllerHit.position, sizeHit, 0f);

        foreach (Collider2D item in obj)
        {
            if (item.CompareTag("Enemy"))
            {
                item
                    .transform
                    .GetComponent<EnemyStatus>()
                    .animator
                    .SetTrigger("hitted");
                item.transform.GetComponent<EnemyStatus>().enemyHealth -=
                    damageHit;

                if (item.transform.GetComponent<EnemyStatus>().enemyHealth <= 0)
                {
                    StartCoroutine(death(item.transform));
                }
            }
        }
    }

    private IEnumerator death(Transform enemy)
    {
        enemy.GetComponent<EnemyMovement>().aiPath.canMove = false;
        enemy.GetComponent<EnemyStatus>().animator.SetTrigger("isDead");

        yield return new WaitForSeconds(3);

        enemy.GetComponent<BoxCollider2D>().size = new Vector3(0.3f, 0, 0);
        enemy.GetComponent<BoxCollider2D>().offset =
            new Vector2(-0.05f, -0.16f);
        CountdownTimer.gameOver = true;
        Time.timeScale = 0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(controllerHit.position, sizeHit);
    }

    void Update()
    {
        if (PhotonNetwork.IsConnected && !view.IsMine) return; // Solo permitir el control para el jugador local o en el modo de un solo jugador

        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetKeyDown(KeyCode.E) && !isAttacking)
        {
            StartCoroutine(Attack());
        }
        else if (Input.GetKeyUp(KeyCode.E) && isAttacking)
        {
            animator.SetBool("isAttacking", false);
            isAttacking = false;
        }

        if (Input.GetKey(KeyCode.R) && !isCharging)
        {
            StartCoroutine(Charge());
        }
        else if (Input.GetKeyUp(KeyCode.R) && isCharging)
        {
            animator.SetBool("Charging", false);
            isCharging = false;
        }

        Flip();

        if (PhotonNetwork.IsConnected && (Vector3) rb.position != lastPosition)
        {
            view
                .RPC("UpdatePlayerPosition",
                RpcTarget.OthersBuffered,
                rb.position);
            lastPosition = rb.position;
        }
    }

    public IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetBool("isAttacking", true);

        audio.clip = hitSound;
        audio.Play();

        yield return new WaitForSeconds(2);
        isAttacking = false;
    }

    public IEnumerator Charge()
    {
        isCharging = true;
        animator.SetBool("Charging", true);
        Animation anim = gameObject.GetComponent<Animation>();
        AnimationState animState = anim[anim.clip.name];

        yield return new WaitForSeconds(2);
        isCharging = false;
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.IsConnected && !view.IsMine)
        {
            rb.position = Vector3.Lerp(rb.position, lastPosition, 0.1f);
            return;
        }

        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(horizontal));
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (
            (isFacingRight && horizontal < 0f) ||
            (!isFacingRight && horizontal > 0f)
        )
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void OnPhotonSerializeView(
        PhotonStream stream,
        PhotonMessageInfo info
    )
    {
        if (stream.IsWriting)
        {
            stream.SendNext(rb.position);
            stream.SendNext(animator.GetBool("isAttacking"));
            stream.SendNext(animator.GetBool("Charging"));
        }
        else
        {
            rb.position = (Vector3) stream.ReceiveNext();
            lastPosition = rb.position;
            animator.SetBool("isAttacking", (bool) stream.ReceiveNext());
            animator.SetBool("Charging", (bool) stream.ReceiveNext());
            lastIsAttacking = animator.GetBool("isAttacking");
            lastIsCharging = animator.GetBool("Charging");
        }
    }

    [PunRPC]
    private void UpdatePlayerPosition(Vector3 newPosition)
    {
        if (!view.IsMine)
        {
            rb.position = newPosition;
            lastPosition = newPosition;
        }
    }
}
