using Unity.VisualScripting;
using UnityEngine;

public class Player : StateOwner
{
    public InputReader inputReader;
    #region Parameters
    [Header("Move On Ground")]
    public float moveSpeed;
    public float groundSlideMultiplier;
    public float xInput { get; private set; }
    public float yInput { get; private set; }

    [Header("Move On Air")]
    public float jumpForce;
    public float movementForceInAir;
    public float airDragMultiplier;
    public float jumpHeightMultiplier;
    [SerializeField] private float coyoteTime = .2f;
    [SerializeField] private float maxFallSpeed;

    public float coyoteTimeCounter { get; set; }

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;

    [Header("Wall Slide")]
    public float wallSlideSpeed = 0.5f;
    public float wallSlideHoldTime;

    [Header("Wall Jump")]
    public float wallJumpTime;
    public float wallJumpForce;
    public Vector2 wallJumpDir;

    [Header("Wall Hop")]
    public float wallHopTime;
    public float wallHopForce;
    public Vector2 wallHopDir;

    private LayerMask enemyLayer;

    [Header("Collision")]
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Vector2 wallCheckSize;
    #endregion
    public bool isBusy { get; private set; }
    public Rigidbody2D Rb { get; private set; }

    protected override void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
    }

    private void InitializeGameInput()
    {

    }

    private void Update()
    {
        ClampVelocity();
        HandleCoyoteTime();
    }

    private void HandleCoyoteTime()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else if (coyoteTimeCounter >= 0)
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
    }

    private void ClampVelocity()
    {
        ClampVelocity(Rb, -maxFallSpeed, float.MaxValue);
    }

    public void ClampVelocity(Rigidbody2D rb, float min, float max)
    {
        Vector2 clampedVelocity = rb.linearVelocity;
        clampedVelocity.y = Mathf.Clamp(rb.linearVelocity.y, min, max);
        rb.linearVelocity = clampedVelocity;
    }

    #region Collision
    public virtual bool IsGrounded() => Physics2D.CircleCast(groundCheck.position, .2f, Vector2.down, groundCheckDistance, groundLayer);

    public virtual bool IsTouchingWall()
    {
        return Physics2D.BoxCast(wallCheck.position, wallCheckSize, 0, transform.right, 0, groundLayer);
    }

    #endregion

    #region Flip
    public void FaceTo(float xValue)
    {
        if (xValue != 0)
        {
            xValue = xValue > 0 ? 1 : -1;
            transform.right = new Vector2(xValue, 0);
        }
    }
    #endregion

    #region Velocity
    public void SetVelocity(float x, float y)
    {
        Rb.linearVelocity = new Vector2(x, y);
        FaceTo(x);
    }
    #endregion
}
