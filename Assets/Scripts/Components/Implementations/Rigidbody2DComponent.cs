using UnityEngine;
using Utils;

public class Rigidbody2DComponent : MonoBehaviour
{
    public Rigidbody2D Rb;

    private void Awake()
    {
        Rb = ComponentCache.GetComponent<Rigidbody2D>(gameObject);
    }

    public Vector2 GetVelocity()
    {
        return Rb.linearVelocity;
    }

    public void SetVelocity(float x, float y)
    {
        Rb.linearVelocity = new Vector2(x, y);
    }
}
