using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private float jumpForce = 0f;
    [SerializeField] private Vector2 deathkick = new Vector2(25f, 25f);

    private float moveInput;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRenderer;
    private Rigidbody2D rb;
    private CapsuleCollider2D cc;
    private BoxCollider2D bc;
    private float gravityScaleAtStart;
    public Transform shootPoint;
    public GameObject bulletPrefab;
    private CinemachineImpulseSource myImpulseSource;

    bool isAlive = true;

    private void Awake() 
    {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        bc = GetComponent<BoxCollider2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        gravityScaleAtStart = rb.gravityScale;  // gravity trong rigidbody
        myImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update() 
    {
        if (!isAlive) return;
        Movement();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
        Shoot();
    }

    private void Movement()
    {
        if (!isAlive) return;
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        // myAnimator.SetBool("IsRunning", playerHasHorizontalSpeed);

        if (moveInput == 0)
            myAnimator.SetBool("IsRunning", false);
        else
            myAnimator.SetBool("IsRunning", true);

    }

    private void Jump()
    {
        if (!isAlive) return;
        if (!bc.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;
        if (Input.GetButtonDown("Jump"))
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    public void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
            transform.localScale = new Vector2(Mathf.Sign(rb.velocity.x), 1f);

        // moveInput = Input.GetAxis("Horizontal");
        // if (moveInput > 0)
        //     mySpriteRenderer.flipX = false;
        //     // transform.localScale = new Vector2(1f, 1f);
        // else if (moveInput < 0)
        //     mySpriteRenderer.flipX = true;
        //     // transform.localScale = new Vector2(-1f, 1f);
    }

    private void ClimbLadder()
    {
        if (!bc.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        {
            rb.gravityScale = gravityScaleAtStart; // Bật lại trọng lực khi rời khỏi cầu thang
            myAnimator.SetBool("IsClimbing", false);
            return;
        }
        rb.gravityScale = 0; // Tắt trọng lực khi leo thang
        float climbInput = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, climbInput * moveSpeed);

        bool playerHasVerticalSpeed = Mathf.Abs(rb.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
    }

    private void Die()
    {
        if (cc.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            Debug.Log("Player died");
            myAnimator.SetTrigger("Dying");
            rb.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            myImpulseSource.GenerateImpulse(1);
        }
    }

    private void Shoot()
    {
        if (!isAlive) return;
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
        }
    }
}
