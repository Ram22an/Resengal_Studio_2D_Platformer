using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private MyMap inputActions;  // Use your input action asset class name
    private Vector2 moveInput;
    private bool jumpPressed;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        inputActions = new MyMap();

        inputActions.PlayerMovement.MovementAndJump.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputActions.PlayerMovement.MovementAndJump.canceled += ctx => moveInput = Vector2.zero;

        // Handle jump input (checking Y axis of movement)
        inputActions.PlayerMovement.MovementAndJump.performed += ctx =>
        {
            if (ctx.ReadValue<Vector2>().y > 0.25f)
                jumpPressed = true;
        };
    }

    void OnEnable()
    {
        inputActions.PlayerMovement.Enable();
    }

    void OnDisable()
    {
        inputActions.PlayerMovement.Disable();
    }

    void FixedUpdate()
    {
        // Horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        // Jump if pressed and grounded
        if (jumpPressed && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }

        jumpPressed = false; // Reset after use
    }

    bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
