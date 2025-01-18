using UnityEngine;
using Utils;

public class PlatformerHorizontalMoveComponent : MonoBehaviour, IMoveable
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    private PlayerController playerController;
    public bool IsMoving = false;

    private void Start()
    {
        rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
        playerController = ComponentCache.GetComponent<PlayerController>(gameObject);

        IsMoving = playerController.InputReader.IsMoving;
    }

    public void Move(Vector3 direction)
    {
        SetVelocity(direction.x * moveSpeed, rb.linearVelocity.y);
    }

    private void SetVelocity(float x,float y)
    {
        rb.linearVelocity = new Vector2(x, y);
    }
}
