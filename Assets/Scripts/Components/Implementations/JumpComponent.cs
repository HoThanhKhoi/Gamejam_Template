using Utils;
using UnityEngine;

public class JumpComponent : MonoBehaviour, IJumpable
{
    [SerializeField] private float jumpForce = 10f;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
    }

    public void Jump()
    {
        rb.AddForce(jumpForce * Vector2.up, ForceMode2D.Impulse);
    }

    public bool IsFalling() => rb.linearVelocity.y < 0f;
}
