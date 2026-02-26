using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D body;
    private PlayerInputActions inputActions;
    private Vector2 moveInput;
    private Animator anim;
    private BoxCollider2D boxCollider;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;

    public float speed = 3f;
    public float jumpForce = 6f;

    private bool jumpPressed;
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
        inputActions.Player.Enable();

        inputActions.Player.Move.performed += ctx =>
            moveInput = ctx.ReadValue<Vector2>();

        inputActions.Player.Move.canceled += ctx =>
            moveInput = Vector2.zero;

        inputActions.Player.Jump.performed += ctx =>
            jumpPressed = true;
    }

    private void OnDisable()
    {
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(moveInput.x * speed, body.linearVelocity.y);
        Flip(moveInput.x);

        const float speedThreshold = 0.05f;
        anim.SetBool("walk", Mathf.Abs(body.linearVelocity.x) > speedThreshold);
        anim.SetBool("grounded", isGrounded());

        if (onWall() && !isGrounded()) {
            float maxFallSpeed = -4f;
            body.linearVelocity = new Vector2(body.linearVelocity.x, maxFallSpeed);
        }


        if (jumpPressed)
        {
            if (isGrounded())
            {
                Jump();
            }
            // Si luego quieres wall jump real, aquí lo añades (ver opción B para rebote)
            jumpPressed = false;
        }
    }

    private void Jump()
    {
        body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        anim.SetBool("grounded", false);
        anim.SetTrigger("jump");
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
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


    private bool isGrounded()
    {
        if (body.linearVelocity.y > 0.01f) return false;

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        if (body.linearVelocity.y > 0.01f) return false;

        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0f, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
}