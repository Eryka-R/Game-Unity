using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Parameters")]
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 6f;

    private Rigidbody2D body;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [Header("Detection")]
    [SerializeField] private BoxCollider2D groundCheck;
    [SerializeField] private BoxCollider2D wallCheck;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    [Header("Wall Slide")]
    [SerializeField] private float wallSlideSpeed = -4f;

    [Header("SFX")]
    [SerializeField] private AudioClip jumpSound;

    private bool jumpPressed;
    private bool attackPressed;
    private bool facingRight = true;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        inputActions = new PlayerInputActions();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnEnable()
    {
        inputActions.Player.Move.performed += OnMovePerformed;
        inputActions.Player.Move.canceled += OnMoveCanceled;
        inputActions.Player.Jump.performed += OnJumpPerformed;
        inputActions.Player.Attack.performed += OnAttackPerformed;

        inputActions.Player.Enable();
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMovePerformed;
        inputActions.Player.Move.canceled -= OnMoveCanceled;
        inputActions.Player.Jump.performed -= OnJumpPerformed;
        inputActions.Player.Attack.performed -= OnAttackPerformed;

        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        if (IsInputBlocked())
        {
            ClearBufferedInputs();
            body.linearVelocity = new Vector2(0f, body.linearVelocity.y);

            const float speedThresholdBlocked = 0.05f;
            anim.SetBool("walk", Mathf.Abs(body.linearVelocity.x) > speedThresholdBlocked);
            anim.SetBool("grounded", IsGrounded());
            return;
        }

        body.linearVelocity = new Vector2(moveInput.x * speed, body.linearVelocity.y);
        Flip(moveInput.x);

        bool grounded = IsGrounded();
        bool wallSliding = IsWallSliding();

        const float speedThreshold = 0.05f;
        anim.SetBool("walk", Mathf.Abs(body.linearVelocity.x) > speedThreshold);
        anim.SetBool("grounded", grounded);

        if (wallSliding)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, wallSlideSpeed);
        }

        if (jumpPressed)
        {
            if (grounded)
            {
                Jump();
            }

            jumpPressed = false;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (IsInputBlocked())
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        if (IsInputBlocked())
        {
            jumpPressed = false;
            return;
        }

        jumpPressed = true;
    }

    private void OnAttackPerformed(InputAction.CallbackContext ctx)
    {
        if (IsInputBlocked())
        {
            attackPressed = false;
            return;
        }

        attackPressed = true;
    }

    private bool IsInputBlocked()
    {
        return GameManager.Instance != null && GameManager.Instance.IsInputBlocked();
    }

    private void ClearBufferedInputs()
    {
        moveInput = Vector2.zero;
        jumpPressed = false;
        attackPressed = false;
    }

    private void Jump()
    {
        SoundManager.instance.PlaySound(jumpSound);
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetBool("grounded", false);
        anim.SetTrigger("jump");
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight)
        {
            facingRight = true;
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
        else if (horizontal < 0 && facingRight)
        {
            facingRight = false;
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x);
            transform.localScale = scale;
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            groundCheck.bounds.center,
            groundCheck.bounds.size,
            0f,
            Vector2.down,
            0.1f,
            groundLayer
        );

        return raycastHit.collider != null && body.linearVelocity.y <= 0.05f;
    }

    private bool IsTouchingWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(
            wallCheck.bounds.center,
            wallCheck.bounds.size,
            0f,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            wallLayer
        );

        RaycastHit2D raycastHit2 = Physics2D.BoxCast(
            wallCheck.bounds.center,
            wallCheck.bounds.size,
            0f,
            new Vector2(transform.localScale.x, 0),
            0.1f,
            groundLayer
        );

        return raycastHit.collider != null || raycastHit2.collider != null;
    }

    private bool IsWallSliding()
    {
        bool grounded = IsGrounded();
        bool touchingWall = IsTouchingWall();

        return touchingWall && !grounded;
    }

    public bool ConsumeAttackInput()
    {
        if (IsInputBlocked())
        {
            attackPressed = false;
            return false;
        }

        if (!attackPressed) return false;

        attackPressed = false;
        return true;
    }

    public bool canAttack()
    {
        return !IsInputBlocked() && moveInput.x == 0 && IsGrounded() && !IsTouchingWall();
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(
                groundCheck.bounds.center,
                groundCheck.bounds.size
            );
        }

        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(
                wallCheck.bounds.center,
                wallCheck.bounds.size
            );
        }
    }
}