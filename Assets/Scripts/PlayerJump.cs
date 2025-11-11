using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    // --- Jump & Physics ---
    private Rigidbody2D rb;
    public float jumpForce = 10f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    // --- Ground Check ---
    public Transform groundCheck;
    public LayerMask groundLayer;
    public float groundCheckRadius = 0.2f;
    private bool isGrounded;

    // --- Coyote Time ---
    private float coyoteTime = 0.15f;
    private float coyoteTimeCounter;

    // --- Jump Buffering ---
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    // --- Double Jump ---
    public int extraJumps = 1;
    private int extraJumpsValue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        extraJumpsValue = extraJumps; // Inicializa los saltos
    }

    void Update()
    {
        // --- Ground Check ---
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // --- Coyote Time & Double Jump Reset ---
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            extraJumpsValue = extraJumps; // Recupera los saltos extra
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // --- Jump Buffering ---
        if (Input.GetButtonDown("Jump"))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // --- COMBINED Jump Input Check ---
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f) // Salto desde suelo (usa coyote)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                coyoteTimeCounter = 0f;
                jumpBufferCounter = 0f;
            }
            else if (extraJumpsValue > 0) // Salto aéreo
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                extraJumpsValue--;
                jumpBufferCounter = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        // --- Better Falling ---
        if (rb.linearVelocity.y < 0)
        {
            // Caída más rápida
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump"))
        {
            // Si suelta el botón durante el ascenso, se acorta el salto
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }

    // Helper function to visualize the ground check radius in the Scene view
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
