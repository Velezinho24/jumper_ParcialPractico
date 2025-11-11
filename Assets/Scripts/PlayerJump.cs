using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody2D rb;

    [Header("Movement")]
    public float moveSpeed = 8f;       // Velocidad de movimiento horizontal
    private float moveInput;           // Valor entre -1 y 1 (A/D o flechas)

    [Header("Jump")]
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    [Header("Jump Mechanics")]
    public float coyoteTime = 0.15f;
    private float coyoteTimeCounter;
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;
    public int extraJumps = 1;
    private int extraJumpsValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsValue = extraJumps;
    }

    void Update()
    {
        // --- Input horizontal ---
        moveInput = Input.GetAxisRaw("Horizontal");

        // --- Ground Check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Coyote Time & Reset ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumpsValue = extraJumps;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffer ---
        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBufferTime;
        else
            jumpBufferCounter -= Time.deltaTime;

        // --- Jump Logic ---
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                Jump();
            }
            else if (extraJumpsValue > 0)
            {
                Jump();
                extraJumpsValue--;
            }
        }
    }

    void FixedUpdate()
    {
        // --- Movimiento horizontal ---
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // --- Mejora de caída ---
        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        coyoteTimeCounter = 0f;
        jumpBufferCounter = 0f;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
