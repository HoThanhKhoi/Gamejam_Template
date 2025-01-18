using UnityEngine;
using Utils;

public class PlatformerHorizontalMoveComponent : MonoBehaviour, IMoveable
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
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
