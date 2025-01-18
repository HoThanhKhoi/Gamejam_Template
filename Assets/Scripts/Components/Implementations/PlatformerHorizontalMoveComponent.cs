using UnityEngine;
using Utils;

public class PlatformerHorizontalMoveComponent : MonoBehaviour, IMoveable
{
    [SerializeField] private float moveSpeed = 5f;
    private IFacing facingComponent;
    private Rigidbody2DComponent rigidbody2DComponent;
    private PlayerPlatformerController playerController;

    private void Start()
    {
        playerController = ComponentCache.GetComponent<PlayerPlatformerController>(gameObject);
        facingComponent = ComponentCache.GetInterface<IFacing>(gameObject);
        rigidbody2DComponent = ComponentCache.GetComponent<Rigidbody2DComponent>(gameObject);
    }

    public void Move()
    {
        Vector2 moveDirection = playerController.InputReader.MoveDirection;
        rigidbody2DComponent.SetVelocity(moveDirection.x * moveSpeed, rigidbody2DComponent.GetVelocity().y);
        facingComponent.SetFacingValue(moveDirection.x);
    }
}
