using UnityEngine;

namespace GameJam.Modules.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class MoveComponent : MonoBehaviour, IMoveable
    {
        public float MoveSpeed = 5f;

        private Rigidbody2D rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Move(Vector2 direction)
        {
            rb.AddForce(direction * MoveSpeed, ForceMode2D.Force);
        }

        public void Stop()
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}