using UnityEngine;
using Utils;

public class PlatformerHorizontalMoveComponent : MonoBehaviour, IMoveable
{
    [SerializeField] private float moveSpeed = 5f;
    private IFacing facingComponent;
    private Rigidbody2DComponent rigidbody2DComponent;
    private Rigidbody2D rb;
    private PlayerController playerController;

    private void Start()
    {
        rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
        playerController = ComponentCache.GetComponent<PlayerController>(gameObject);
        facingComponent = ComponentCache.GetInterface<IFacing>(gameObject);
    }

    public void Move()
    {
        Vector2 moveDirection = playerController.InputReader.MoveDirection;
        SetVelocity(moveDirection.x * moveSpeed, rb.linearVelocity.y);
        facingComponent.SetFacingValue(moveDirection.x);
    }
}
