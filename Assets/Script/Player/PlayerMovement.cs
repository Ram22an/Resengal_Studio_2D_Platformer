using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private MyMap inputActions;  // Use your input action asset class name
    private Vector2 moveInput;
    private bool jumpPressed;

    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField]private Transform MyTransforms;


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
        Vector3 scale = MyTransforms.localScale;
        scale.x = moveInput.x > 0 ? 1 : moveInput.x < 0 ? -1 : scale.x;
        MyTransforms.localScale = scale;

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
