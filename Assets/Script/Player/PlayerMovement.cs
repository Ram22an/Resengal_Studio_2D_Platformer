using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private MyMap inputActions;  // Use your input action asset class name
    private Vector2 moveInput;
    private bool jumpPressed;

    [Header("Movement Settings")]
    [SerializeField]private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    public int maxJumps = 1;        
    private int currentJumpCount = 0;

    [SerializeField]private Transform MyTransforms;

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * 0.2f);
    }


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
        scale.x = moveInput.x > 0 ? 0.8f : moveInput.x < 0 ? -0.8f : scale.x;
        MyTransforms.localScale = scale;

        // Horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        if (IsGrounded())
        {
            currentJumpCount = 0;
        }

        // Jump if pressed and grounded
        if (jumpPressed && currentJumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            currentJumpCount++;
            jumpPressed = false; // reset jump input
        }

    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, 0.2f, groundLayer);
        return hit.collider != null;
    }

}

