using UnityEngine;
using Utils;

public class TopDownMoveComponent : MonoBehaviour, IMoveable
{
    [SerializeField] private float speed = 5f;
    private IFacing facingComponent;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
        facingComponent = ComponentCache.GetInterface<IFacing>(gameObject);
    }

    public void Move()
    {
        Vector2 moveDirection = ComponentCache.GetComponent<InputReaderComponent>(gameObject).InputReader.MoveDirection;
        
        facingComponent.SetFacingValue(moveDirection.x);

        SetVelocity(moveDirection.x, moveDirection.y);
    }

    private void SetVelocity(float x, float y)
    {
        Vector2 velocity = new Vector2(x, y) * speed;
        rb.linearVelocity = velocity;
    }

}
