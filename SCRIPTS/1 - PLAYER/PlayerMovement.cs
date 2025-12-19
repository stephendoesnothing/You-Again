using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer playerVisuals;
    [SerializeField] private Animator animator;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float deceleration = 15f;
    [SerializeField] private bool airControl = true;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private float variableJumpHeight = 0.5f;
    [SerializeField] private int maxAirJumps = 1;
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float bufferTime = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashForce = 40f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool canDash = false;

    [Header("Checks")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Invincibility")]
    public bool isInvulnerable = false;
    public float invincibilityDuration = 1f;

    // Private variables

    private Rigidbody2D rb;
    private Vector2 velocity;
        
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool canMove = true;
    private bool isDashing;
    private float airTime;
    private bool isFalling;


    private float coyoteTimeCounter;
    private float jumpBufferCounter;

    private int jumpPhase;
    private float horizontalInput;
    private float dashCooldownTimer;

    private bool isWalkingSoundPlaying = false;


    private void HandleWalkSound()
    {
        bool isWalking = isGrounded && Mathf.Abs(horizontalInput) > 0.1f && !isDashing && canMove;

        if (isWalking)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                SoundManager.Instance.PlaySFX("Walk");
                stepTimer = stepDelay;
            }
        }
        else
        {
            stepTimer = 0f; // reset so the sound plays immediately when walking resumes
        }
    }


    [SerializeField] private float stepDelay = 0.4f; // Delay between steps (adjust for your game's speed)
    private float stepTimer = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        CheckGround();
        HandleInput();
        HandleTimers();
        HandleJump();
        HandleVariableJump();
        HandleDashInput();
        UpdateAnimations();
        HandleWalkSound();

        if (!isGrounded)
        {
            airTime += Time.deltaTime;
        }
        else
        {
            airTime = 0f;
        }

        if (!isGrounded)
        {
            airTime += Time.deltaTime;

            // Start falling only if enough time has passed or velocity is negative
            isFalling = airTime > 0.1f && rb.velocity.y <= 0f;
        }
        else
        {
            airTime = 0f;
            isFalling = false;
        }
    }

    private void FixedUpdate()
    {
        if (canMove && !isDashing) Move();
    }

    private void HandleInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump")) jumpBufferCounter = bufferTime;

        if (Input.GetButtonUp("Jump")) jumpReleased = true;
        else jumpReleased = false;
    }

    private bool jumpReleased;

    private void HandleTimers()
    {
        jumpBufferCounter -= Time.deltaTime;
        coyoteTimeCounter -= Time.deltaTime;
        dashCooldownTimer -= Time.deltaTime;
    }

    private void Move()
    {

        float targetSpeed = horizontalInput * moveSpeed;
        float speedDiff = targetSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, 0.9f) * Mathf.Sign(speedDiff);

        rb.AddForce(movement * Vector2.right);
    }

    private void HandleJump()
    {
        if (jumpBufferCounter > 0 && (coyoteTimeCounter > 0 || jumpPhase < maxAirJumps))
        {            
            PerformJump();
            jumpBufferCounter = 0;
            animator.SetTrigger("Jump");
            SoundManager.Instance.PlaySFX("Jump");
        }
    }

    private void PerformJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        jumpPhase++;
        coyoteTimeCounter = 0;
    }

    private void HandleVariableJump()
    {
        if (jumpReleased && rb.velocity.y > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeight);
        }
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));
        animator.SetBool("isGrounded", isGrounded);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            jumpPhase = 0;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    public void UnlockDash()
    {
        canDash = true;
    }

    public void UnlockDoubleJump()
    {
        maxAirJumps = 1; // default is 0, this allows for a double jump
    }

    public void UnlockHigherJump()
    {
        jumpForce += 10f; // increases jump height, tweak as needed
    }


    private void HandleDashInput()
    {
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0 && !isDashing)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        SoundManager.Instance.PlaySFX("Dash");
        isDashing = true;
        canMove = false;
        isInvulnerable = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        Color color = playerVisuals.color;
        color.a = 0.5f;
        playerVisuals.color = color;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dashDir = (mouseWorldPos - transform.position).normalized;

        rb.velocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashDuration);
      
        color.a = 1f;
        playerVisuals.color = color;
        isInvulnerable = false;
        rb.gravityScale = originalGravity;
        canMove = true;
        isDashing = false;
        dashCooldownTimer = dashCooldown;
    }

    public void TriggerInvincibility()
    {
        if (!isInvulnerable)
        {
            StartCoroutine(InvincibilityRoutine());
        }
    }

    private IEnumerator InvincibilityRoutine()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvulnerable = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
