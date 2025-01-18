using UnityEngine;
using Utils;

public class Rigidbody2DComponent : MonoBehaviour
{
    public Rigidbody2D rb;

    private void Awake()
    {
        rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
    }

    private Vector2 GetVelocity()
    {
        return rb.linearVelocity;
    }

    private void SetVelocity(float x, float y)
    {
        rb.linearVelocity = new Vector2(x, y);
    }
}
